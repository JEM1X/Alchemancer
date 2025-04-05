using System;
using System.Collections;
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

    // События
    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;

    private int currentWave = 0;
    private bool isPlayerTurn = true;
    private bool isBattleOver = false;
    private bool isPlayerMoveCompleted = false;
    private bool isEnemyTurnInProgress = false;
    private bool isWaveInProgress = false;

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
        for (currentWave = 1; currentWave <= totalWaves; currentWave++)
        {
            Debug.Log($"Начинается волна {currentWave}/{totalWaves}");
            isWaveInProgress = true;

            horde.SpawnEnemy();
            horde.OnNoEnemyLeft += OnWaveCleared;

            StartPlayerTurn();

            while (isWaveInProgress && !isBattleOver)
                yield return null;

            if (isBattleOver) yield break;

            yield return new WaitForSeconds(waveDelay);
        }

        Victory();
    }

    public void StartPlayerTurn()
    {
        if (isBattleOver) return;

        isPlayerTurn = true;
        isPlayerMoveCompleted = false;
        Debug.Log($"Ход игрока (волна {currentWave})");

        // Вызываем событие начала хода игрока
        OnPlayerTurnStarted?.Invoke();
    }

    public void CompletePlayerTurn()
    {
        if (!isPlayerTurn || isPlayerMoveCompleted || isBattleOver) return;

        isPlayerMoveCompleted = true;
        isPlayerTurn = false;
        Debug.Log("Ход игрока завершен");

        StartEnemyTurn();
    }

    private void StartEnemyTurn()
    {
        if (isBattleOver || isEnemyTurnInProgress || !isWaveInProgress) return;

        isEnemyTurnInProgress = true;
        Debug.Log($"Ход врагов (волна {currentWave})");

        // Вызываем событие начала хода врагов
        OnEnemyTurnStarted?.Invoke();

        StartCoroutine(ExecuteEnemyTurns());
    }

    private IEnumerator ExecuteEnemyTurns()
    {
        foreach (var enemy in horde.EnemyScripts)
        {
            if (isBattleOver || !isWaveInProgress) yield break;

            bool enemyTurnCompleted = false;
            yield return new WaitForSeconds(3f);
            enemy.TakeTurn(() => enemyTurnCompleted = true);

            while (!enemyTurnCompleted && !isBattleOver)
                yield return null;

            enemy.ReduceStatusEffects();
        }

        isEnemyTurnInProgress = false;

        if (!isBattleOver && isWaveInProgress)
            StartPlayerTurn();
    }

    private void OnWaveCleared()
    {
        if (!isWaveInProgress || isBattleOver) return;

        Debug.Log($"Волна {currentWave} пройдена!");
        horde.OnNoEnemyLeft -= OnWaveCleared;
        isWaveInProgress = false;
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
        Debug.Log("Победа! Все волны пройдены!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Defeat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        player.OnDeath -= OnPlayerDeath;
        horde.OnNoEnemyLeft -= OnWaveCleared;

        // Очищаем все подписки
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