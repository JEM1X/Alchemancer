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
            Debug.LogError("Horde �� �������� � GameManager!");
            return;
        }

        // ������������� �� ������� ������ ����� ������
        _horde.OnNewEnemy += AddEnemyToQueue;

        // ��������� ��� ������������ ������
        foreach (var enemy in _horde.EnemyScripts)
        {
            AddEnemyToQueue(enemy);
        }
    }
    private void InitializeCombat()
    {
        turnQueue.Clear(); // ������ ������� ����� �����������

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
            InitializeCombat(); // ������������� ������� ����, ���� ��� ������
            return;
        }

        Combatant current = turnQueue.Dequeue();

        if (current == null || current.Health <= 0)
        {
            NextTurn(); // ���������� �������
            return;
        }

        if (current is PlayerCombat)
        {
            isPlayerTurn = true;
            Debug.Log("��� ������!");
            // ���� �������� ������ (������������� �����, ������� ���� � �.�.)
        }
        else if (current is Enemy enemy)
        {
            isPlayerTurn = false;
            Debug.Log($"����� ���� {enemy.name}");
            enemy.TakeTurn(() => NextTurn()); // ���� ������ ���, ����� �������� ����������
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
            Debug.Log("��� ����� �� ������ ���������");
            HandleWaveEnd();
            return;
        }

        Enemy target = _horde.EnemyScripts[0]; // ����� ������� ����� � ������
        int damage = 10; // ���� ������ (����� ������� ������������)

        target.TakeDamage(damage);
        Debug.Log($"����� �������� {target.name} � ����� {damage} �����!");

        if (target.Health <= 0)
        {
            _horde.EnemyScripts.Remove(target); // ������� ����� �� ������, ���� �� ����
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
        Debug.Log("����� ���������!");

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
        // ����� ����� �������� ������: ��������, ������� �� ��������� ������� ����� 3 ����
        
        //}
        return false;
    }

    private void StartNewWave()
    {
        Debug.Log("���������� ����� �����!");
        _horde.SpawnEnemy();
        InitializeCombat(); // ��������� ����� ����� ���
    }

    private void LoadNextLevel()
    {
        Debug.Log("������� �� ��������� �������!");
        // ������ �������� ���������� ������, ��������:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
