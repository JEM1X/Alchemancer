using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerCombat player;
    [SerializeField] private List<Enemy> enemies;
    public List<Enemy> Enemies { get; }
    [SerializeField] private Horde _horde;
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
        foreach (var enemy in _horde.EnemyScripts)
        {
            AddEnemyToQueue(enemy);
        }
    }
    private void InitializeCombat()
    {
        turnQueue.Clear(); // Чистим очередь перед заполнением

        turnQueue.Enqueue(player);

        foreach (var enemy in _horde.EnemyScripts)
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

        if (current is PlayerCombat)
        {
            isPlayerTurn = true;
            Debug.Log("Ход игрока!");
            // Ждем действий игрока (использование зелья, пропуск хода и т.д.)
        }
        else if (current is Enemy enemy)
        {
            isPlayerTurn = false;
            Debug.Log($"Ходит враг {enemy.name}");
            enemy.TakeTurn(() => NextTurn()); // Враг делает ход, затем передает управление
        }
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

        Enemy target = _horde.EnemyScripts[0]; // Берем первого врага в списке
        int damage = 10; // Урон игрока (можно сделать динамическим)

        target.TakeDamage(damage);
        Debug.Log($"Игрок атаковал {target.name} и нанес {damage} урона!");

        if (target.Health <= 0)
        {
            _horde.EnemyScripts.Remove(target); // Удаляем врага из списка, если он умер
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
        //if (HordeCounter > 4) 
        //{
        // Здесь можно добавить логику: например, переход на следующий уровень после 3 волн
        
        //}
        return false;
    }

    private void StartNewWave()
    {
        Debug.Log("Начинается новая волна!");
        _horde.SpawnEnemy();
        InitializeCombat(); // Запускаем новую волну боя
    }

    private void LoadNextLevel()
    {
        Debug.Log("Переход на следующий уровень!");
        // Логика загрузки следующего уровня, например:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
