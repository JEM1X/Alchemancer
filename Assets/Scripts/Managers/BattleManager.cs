using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Battle Settings")]
    [SerializeField] private Horde horde;
    public Combatant Player => player;
    [SerializeField] private Combatant player;
    public int TotalWaves { get => totalWaves; }
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float enemyAttackDelay = 2f;
    private int currentWave = 0;
    private bool isWaveInProgress = false;


    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;
    public event Action OnAllWavesCleared;
    public event Action OnPlayerLose;
    public event Action<int> OnWaveStart;

    private void Start()
    {
        OnPlayerTurnStarted += () => Debug.Log("Тест: событие игрока получено");
        
        BattleLogic();
    }
    private void BattleLogic() 
    {
        if (horde.EnemyScripts.Count == 0) 
        {
            isWaveInProgress = false;   
        }
        if (!isWaveInProgress && currentWave < totalWaves)
        {
            horde.SpawnEnemy();
            currentWave += 1;
            OnWaveStart?.Invoke(currentWave);
            isWaveInProgress = true;
        }
        else if (currentWave >= totalWaves && horde.EnemyScripts.Count == 0)
        {
            Victory();
            return;
        }

        
        StartCoroutine(EachEnemyTurn());
        



    }
    public void StartPlayerTurn() 
    {
        OnPlayerTurnStarted?.Invoke();
        Debug.Log("Ход игрока начался");
    }
    public void CompletePlayerTurn() 
    {
        BattleLogic();
        Debug.Log("Ход игрока закончен");
    }
    private void Victory() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator EachEnemyTurn() 
    {
        OnEnemyTurnStarted?.Invoke();
        Debug.Log("Ход врага");
        foreach (var enemy in horde.EnemyScripts)
        {
            yield return new WaitForSeconds(enemyAttackDelay);
            bool turnCompleted = false;
            enemy.TakeTurn(() => turnCompleted = true);
            enemy.ReduceStatusEffects();

        }
        yield return new WaitForSeconds(enemyAttackDelay);
        StartPlayerTurn();
    }
}
