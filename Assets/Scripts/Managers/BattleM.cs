using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleM : Singleton<BattleM>
{
    [Header("Battle Settings")]
    public Horde Horde { get => horde; }
    [SerializeField] private Horde horde;
    public AlchemancerMediator Mediator => mediator;
    [SerializeField] private AlchemancerMediator mediator;
    public int TotalWaves { get => totalWaves; }
    [SerializeField] private int totalWaves = 3;
    private int currentWave = 0;
    private float turnDelay = 0.25f;

    public event Action OnEnemiesTurnStarted;
    public event Action OnAllWavesCleared;
    public event Action<int> OnWaveStart;


    private void Start()
    {
        WaveTracker();
    }

    private void WaveTracker()
    {

        if (horde.EnemyScripts.Count == 0 && currentWave < totalWaves)
        {
            horde.SpawnEnemy();
            currentWave += 1;
            OnWaveStart?.Invoke(currentWave);
        }
        else if (currentWave >= totalWaves && horde.EnemyScripts.Count == 0)
        {
            OnAllWavesCleared();
            return;
        }

        StartCoroutine(BattleSequence());
    }

    private IEnumerator BattleSequence()
    {
        yield return null;

        foreach (var enemy in Horde.EnemyScripts) 
        {
            enemy.PlanNextAction();
        }

        Debug.Log("Player Turn");
        yield return StartCoroutine(WaitForTurn(mediator.PlayerCombat));

        OnEnemiesTurnStarted?.Invoke();
        Debug.Log("Enemy Turn");

        for (int i = horde.EnemyScripts.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(turnDelay);

            Enemy enemy = horde.EnemyScripts[i];
            yield return StartCoroutine(WaitForTurn(enemy));
        }

        WaveTracker();
    }

    private IEnumerator WaitForTurn(Combatant combatant)
    {
        bool isTurnCompleted = false;
        Action CompleteTurn = () => isTurnCompleted = true;

        StartCoroutine(combatant.TakeTurn(CompleteTurn));

        yield return new WaitUntil(() => isTurnCompleted);
    }
}
