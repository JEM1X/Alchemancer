using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BM : MonoBehaviour
{
    public static BM Instance;

    [Header("Battle Settings")]
    [SerializeField] private Horde horde;
    [SerializeField] private Combatant player;
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float waveDelay = 2f;

    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;

    private int currentWave = 0;
    private bool isPlayerTurn = true;
    private bool isBattleOver = false;
    private bool isPlayerMoveCompleted = false;
    private bool isEnemyTurnInProgress = false;
    private bool isWaveInProgress = false;
    private bool isWaveCleared = false;
    public event Action OnAllWavesCleared;
    public event Action OnPlayerLose;
    public event Action<int> OnWaveClear;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player.OnDeath += OnPlayerDeath;
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        currentWave = 1;
        yield return ContinueBattleLoop();
    }

    private IEnumerator ContinueBattleLoop()
    {
        yield return new WaitForSeconds(waveDelay);

        if (currentWave > totalWaves)
        {
            Victory();
            yield break;
        }

        Debug.Log($"Начинается волна {currentWave}/{totalWaves}");
        isWaveInProgress = true;

        horde.SpawnEnemy();
        horde.OnNoEnemyLeft += OnWaveCleared;

        if (horde.EnemyScripts.Count == 0)
        {
            OnWaveCleared();
            yield break;
        }

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (isBattleOver) return;

        isPlayerTurn = true;
        isPlayerMoveCompleted = false;
        Debug.Log($"Ход игрока (волна {currentWave})");

        OnPlayerTurnStarted?.Invoke();
        player.ReduceStatusEffects();
    }

    public void CompletePlayerTurn()
    {
        if (!isPlayerTurn || isPlayerMoveCompleted || isBattleOver) return;

        isPlayerMoveCompleted = true;
        isPlayerTurn = false;
        Debug.Log("Ход игрока завершен");

        if (isWaveCleared)
        {
            isWaveCleared = false;
            currentWave++;
            if (currentWave <= totalWaves)
            {
                StartCoroutine(ContinueBattleLoop());
            }
            else
            {
                Victory();
            }
        }
        else
        {
            StartEnemyTurn();
        }
    }

    private void StartEnemyTurn()
    {
        if (isBattleOver || isEnemyTurnInProgress || !isWaveInProgress) return;

        isEnemyTurnInProgress = true;
        Debug.Log($"Ход врагов (волна {currentWave})");

        OnEnemyTurnStarted?.Invoke();

        StartCoroutine(ExecuteEnemyTurns());
    }

    private IEnumerator ExecuteEnemyTurns()
    {
        // Создаем копию списка живых врагов
        var enemiesToProcess = new List<Enemy>(horde.EnemyScripts.FindAll(e => e != null && e.Health > 0));

        if (enemiesToProcess.Count == 0)
        {
            isEnemyTurnInProgress = false;
            OnWaveCleared();
            yield break;
        }

        foreach (var enemy in enemiesToProcess)
        {
            if (isBattleOver || !isWaveInProgress) yield break;
            if (enemy != null && enemy.Health > 0)
            {
                enemy.ReduceStatusEffects(); 
                if (enemy == null || enemy.Health <= 0)
                {
                    if (!horde.EnemyScripts.Exists(e => e != null && e.Health > 0))
                    {
                        OnWaveCleared();
                        yield break;
                    }
                    continue;
                }
            }

            yield return new WaitForSeconds(1f);

            // Повторная проверка после ожидания
            if (enemy == null || enemy.Health <= 0) continue;

            bool turnCompleted = false;
            enemy.TakeTurn(() => turnCompleted = true);

            while (!turnCompleted && !isBattleOver && enemy != null && enemy.Health > 0)
                yield return null;

            // Проверяем состояние волны после каждого хода
            if (!horde.EnemyScripts.Exists(e => e != null && e.Health > 0))
            {
                OnWaveCleared();
                yield break;
            }
        }

        isEnemyTurnInProgress = false;

        if (!isBattleOver && isWaveInProgress && horde.EnemyScripts.Exists(e => e != null && e.Health > 0))
        {
            StartPlayerTurn();
        }
    }

    private void OnWaveCleared()
    {
        if (!isWaveInProgress || isBattleOver) return;

        Debug.Log($"Волна {currentWave} пройдена!");
        horde.OnNoEnemyLeft -= OnWaveCleared;
        isWaveInProgress = false;
        isWaveCleared = true;

        // Если был ход врагов - завершаем его
        if (isEnemyTurnInProgress)
        {
            isEnemyTurnInProgress = false;
        }

        // Если был ход игрока - завершаем его
        if (isPlayerTurn)
        {
            CompletePlayerTurn();
        }
        else
        {
            // Если волна очищена во время хода врагов
            currentWave++;
            if (currentWave <= totalWaves)
            {
                StartCoroutine(ContinueBattleLoop());
            }
            else
            {
                Victory();
            }
        }
    }

    private void OnPlayerDeath()
    {
        if (isBattleOver) return;

        isBattleOver = true;
        Debug.Log("Игрок погиб!");
        Defeat();
    }

    private void Victory()
    {
        isBattleOver = true;
        OnAllWavesCleared?.Invoke();
    }

    private void Defeat()
    {
        OnPlayerLose?.Invoke();
    }

    private void OnDestroy()
    {
        player.OnDeath -= OnPlayerDeath;
        horde.OnNoEnemyLeft -= OnWaveCleared;

        OnPlayerTurnStarted = null;
        OnEnemyTurnStarted = null;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (player == null)
        {
            Debug.LogWarning("Игрок не назначен в BattleManager!");
            return;
        }

        player.TakeDamage(damage);
    }
}
