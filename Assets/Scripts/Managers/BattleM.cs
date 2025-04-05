using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleM : Singleton<BattleM>
{
    [Header("Battle Settings")]
    [SerializeField] private Horde horde;
    [SerializeField] private Combatant player;
    public int TotalWaves { get => totalWaves; }
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float waveDelay = 2f;
     
    private int currentWave = 0;
    private bool isPlayerTurn = true;
    private bool isBattleOver = false;
    private bool isPlayerMoveCompleted = false;
    private bool isEnemyTurnInProgress = false;
    private bool isWaveInProgress = false;
    private bool isWaveCleared = false;


    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;
    public event Action OnAllWavesCleared;
    public event Action OnPlayerLose;
    public event Action<int> OnWaveStart;

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
        isWaveCleared = false;

        horde.SpawnEnemy();
        OnWaveStart?.Invoke(currentWave);
        horde.OnNoEnemyLeft += OnWaveCleared;

        if (!HasAliveEnemies())
        {
            Debug.LogWarning("Не удалось заспавнить врагов!");
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

        if (!HasAliveEnemies())
        {
            OnWaveCleared();
            return;
        }

        if (!isWaveCleared)
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
        var enemiesToProcess = GetAliveEnemies();

        if (enemiesToProcess.Count == 0)
        {
            isEnemyTurnInProgress = false;
            OnWaveCleared();
            yield break;
        }

        foreach (var enemy in enemiesToProcess)
        {
            if (isBattleOver || !isWaveInProgress) yield break;

            if (enemy == null || enemy.Health <= 0) continue;

            enemy.ReduceStatusEffects();

            if (enemy.Health <= 0 || !HasAliveEnemies())
            {
                OnWaveCleared();
                yield break;
            }

            yield return new WaitForSeconds(1f);

            if (enemy == null || enemy.Health <= 0) continue;

            bool turnCompleted = false;
            enemy.TakeTurn(() => turnCompleted = true);

            yield return new WaitUntil(() => turnCompleted || isBattleOver || enemy == null || enemy.Health <= 0);

            if (!HasAliveEnemies())
            {
                OnWaveCleared();
                yield break;
            }
        }

        isEnemyTurnInProgress = false;

        if (!isBattleOver && isWaveInProgress && HasAliveEnemies())
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

        StopAllCoroutines();
        isEnemyTurnInProgress = false;

        // Сохраняем текущий номер волны перед инкрементом
        int clearedWave = currentWave;
        currentWave++;

        //OnWaveStart?.Invoke(clearedWave);

        if (currentWave > totalWaves)
        {
            Victory();
        }
        else
        {
            // Всегда запускаем новую волну через ContinueBattleLoop
            StartCoroutine(ContinueBattleLoop());
            StartPlayerTurn();
            CompletePlayerTurn();
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
        OnAllWavesCleared = null;
        OnPlayerLose = null;
        OnWaveStart = null;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (player == null)
        {
            Debug.LogWarning("Игрок не назначен в BattleManager!");
            return;
        }

        player.TakeDamage(damage);
        AudioM.Instance.PlaySound(AudioM.Instance.punchSounds[UnityEngine.Random.Range(0, AudioM.Instance.punchSounds.Length)]);
    }

    // Вспомогательные методы для проверки живых врагов
    private bool HasAliveEnemies()
    {
        return horde.EnemyScripts.Exists(e => e != null && e.Health > 0);
    }

    private List<Enemy> GetAliveEnemies()
    {
        return horde.EnemyScripts.FindAll(e => e != null && e.Health > 0);
    }
}