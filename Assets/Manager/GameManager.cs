using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private List<Enemy> enemies;
    public List<Enemy> Enemies { get; }
    [SerializeField] private Horde _horde;
    private Queue<Combatant> turnQueue = new Queue<Combatant>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
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

        // Подписываемся на событие спавна новых врагов
        _horde.OnNewEnemy += AddEnemyToQueue;

        // Добавляем уже заспавненных врагов
        foreach (var enemy in _horde.EnemyScript)
        {
            AddEnemyToQueue(enemy);
        }
    }
    private void InitializeCombat()
    {
        turnQueue.Clear(); // Чистим очередь перед заполнением

        turnQueue.Enqueue(player);

        foreach (var enemy in _horde.EnemyScript)
        {
            turnQueue.Enqueue(enemy);
        }

        NextTurn();
    }

    private void AddEnemyToQueue(Enemy enemy)
    {
        if (enemy == null) return;
        turnQueue.Enqueue(enemy);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EndPlayerTurn();


        }
    }
    private void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            InitializeCombat(); // Перезапускаем ходовой цикл, если все прошли
            return;
        }

        Combatant current = turnQueue.Dequeue();

        if (current == null || current.Health <= 0)
        {
            NextTurn(); // Пропускаем мертвых
            return;
        }

        if (current is Player)
        {
            Debug.Log("Ход игрока!");
            // Ждем действий игрока (использование зелья, пропуск хода и т.д.)
        }
        else if (current is Enemy enemy)
        {
            Debug.Log($"Ходит враг {enemy.name}");
            enemy.TakeTurn(() => NextTurn()); // Враг делает ход, затем передает управление
        }
    }

    public void EndPlayerTurn()
    {
        NextTurn();
    }


    public void PlayerTakeDamage(int damage)
    {
        player.TakeDamage(damage);
        if (player.Health <= 0)
        {
            Debug.Log("Игрок проиграл! Поражение.");
            return;
        }
    }

}
