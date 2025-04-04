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

    //[Header("Game Over UI")]
    //[SerializeField] private GameObject victoryScreen;
    //[SerializeField] private GameObject defeatScreen;

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

            // Спавн новой волны врагов
            horde.SpawnEnemy();
            horde.OnNoEnemyLeft += OnWaveCleared;

            // Начинаем бой волны
            StartPlayerTurn();

            // Ждем завершения волны (либо победы, либо поражения)
            while (isWaveInProgress && !isBattleOver)
                yield return null;

            if (isBattleOver) yield break;

            // Задержка между волнами
            yield return new WaitForSeconds(waveDelay);
        }

        // Все волны пройдены
        Victory();
    }

    public void StartPlayerTurn()
    {
        if (isBattleOver) return;

        isPlayerTurn = true;
        isPlayerMoveCompleted = false;
        Debug.Log($"Ход игрока (волна {currentWave})");

        // Активируем UI для хода игрока
        // Например: EnablePlayerUI(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isPlayerTurn) 
        {
            CompletePlayerTurn();
        }
    }
    public void CompletePlayerTurn()
    {
        if (!isPlayerTurn || isPlayerMoveCompleted || isBattleOver) return;

        isPlayerMoveCompleted = true;
        isPlayerTurn = false;
        Debug.Log("Ход игрока завершен");

        // Деактивируем UI для хода игрока
        // Например: EnablePlayerUI(false);

        StartEnemyTurn();
    }

    private void StartEnemyTurn()
    {
        if (isBattleOver || isEnemyTurnInProgress || !isWaveInProgress) return;

        isEnemyTurnInProgress = true;
        Debug.Log($"Ход врагов (волна {currentWave})");

        StartCoroutine(ExecuteEnemyTurns());
    }

    private IEnumerator ExecuteEnemyTurns()
    {
        foreach (var enemy in horde.EnemyScripts)
        {
            if (isBattleOver || !isWaveInProgress) yield break;

            bool enemyTurnCompleted = false;
            enemy.TakeTurn(() => enemyTurnCompleted = true);

            while (!enemyTurnCompleted && !isBattleOver)
                yield return null;
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
        //victoryScreen.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Defeat()
    {
        //defeatScreen.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        player.OnDeath -= OnPlayerDeath;
        horde.OnNoEnemyLeft -= OnWaveCleared;
    }
}