using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] private PlayerCombat player;
    [SerializeField] private Horde _horde;
    [SerializeField] private int Maxhordes;

    private Queue<Combatant> turnQueue = new Queue<Combatant>();
    private bool isPlayerTurn = false;
    private int HordeCounter = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (player != null)
            player.OnDeath += OnPlayerDeath;

        RegisterHorde();
        InitializeCombat();
    }

    private void RegisterHorde()
    {
        if (_horde == null)
        {
            Debug.LogError("Horde не назначен в GameManager!");
            return;
        }

        _horde.OnNewEnemy += AddEnemyToQueue;

        foreach (var enemy in _horde.EnemyScripts)
        {
            AddEnemyToQueue(enemy);
        }
    }

    private void InitializeCombat()
    {
        turnQueue.Clear();

        if (player != null && player.Health > 0)
            turnQueue.Enqueue(player);

        foreach (var enemy in _horde.EnemyScripts)
        {
            if (enemy != null && enemy.Health > 0)
                turnQueue.Enqueue(enemy);
        }

        NextTurn();
    }

    private void AddEnemyToQueue(Enemy enemy)
    {
        if (enemy == null || enemy.Health <= 0) return;
        turnQueue.Enqueue(enemy);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isPlayerTurn)
        {
            PlayerAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && isPlayerTurn)
        {
            NextTurn();
        }
    }

    private void NextTurn()
    {
        while (turnQueue.Count > 0)
        {
            Combatant current = turnQueue.Dequeue();

            if (current == null || current.Health <= 0)
                continue;

            if (current is PlayerCombat)
            {
                isPlayerTurn = true;
                Debug.Log("Ход игрока!");
                return;
            }
            else if (current is Enemy enemy)
            {
                isPlayerTurn = false;
                Debug.Log($"Ходит враг {enemy.name}");
                enemy.TakeTurn(() => NextTurn());
                return;
            }
        }

        Debug.Log("Очередь пуста. Начинаем новый цикл.");
        InitializeCombat();
    }

    public void EndPlayerTurn()
    {
        NextTurn();
    }

    private void PlayerAttack()
    {
        if (_horde.EnemyScripts.Count == 0)
        {
            Debug.Log("Все враги на уровне повержены");
            HandleWaveEnd();
            return;
        }

        Enemy target = _horde.EnemyScripts[0];
        int damage = 10;

        target.TakeDamage(damage);
        Debug.Log($"Игрок атаковал {target.name} и нанес {damage} урона!");

        if (target.Health <= 0)
        {
            _horde.EnemyScripts.Remove(target);
        }

        if (_horde.EnemyScripts.Count == 0)
        {
            HandleWaveEnd();
        }
        else
        {
            NextTurn();
        }
    }

    private void HandleWaveEnd()
    {
        Debug.Log("Волна завершена!");

        if (ShouldGoToNextLevel())
        {
            LoadNextLevel();
        }
        else
        {
            StartNewWave();
            HordeCounter++;
        }
    }

    private bool ShouldGoToNextLevel()
    {
        return HordeCounter >= Maxhordes;
    }

    private void StartNewWave()
    {
        Debug.Log("Начинается новая волна!");
        _horde.SpawnEnemy();
        InitializeCombat();
    }

    private void LoadNextLevel()
    {
        Debug.Log("Переход на следующий уровень!");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    private void OnPlayerDeath()
    {
        Debug.Log("Игрок погиб!");

        // Удаляем игрока из очереди
        Queue<Combatant> newQueue = new Queue<Combatant>();
        while (turnQueue.Count > 0)
        {
            var unit = turnQueue.Dequeue();
            if (unit != player && unit != null && unit.Health > 0)
                newQueue.Enqueue(unit);
        }
        turnQueue = newQueue;

        // Завершаем бой, можно заменить на экран "поражение"
        Debug.Log("Бой окончен. Игра окончена.");

        // Здесь можно вызвать GameOver экран или что-то другое
        // SceneManager.LoadScene("GameOverScene");
    }
    private void OnDestroy()
    {
        if (player != null)
            player.OnDeath -= OnPlayerDeath;

        if (_horde != null)
            _horde.OnNewEnemy -= AddEnemyToQueue;
    }
}
