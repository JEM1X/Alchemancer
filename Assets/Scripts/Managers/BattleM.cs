using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleM : Singleton<BattleM>
{
    [Header("Battle Settings")]
    public Horde Horde { get => horde; }
    [SerializeField] private Horde horde;
    public Alchemancer Alchemancer => alchemancer;
    [SerializeField] private Alchemancer alchemancer;
    public int TotalWaves { get => totalWaves; }
    [Tooltip("����� ���������� ���� (������������, ���� isInfiniteMode = true)")]
    [SerializeField] private int totalWaves = 3;

    public bool IsInfiniteMode { get => isInfiniteMode; }
    [Tooltip("����������� �����? ���� true, totalWaves ������������")]
    [SerializeField] private bool isInfiniteMode = false;
    public int CurrentWave { get => currentWave; }
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
        if (horde.EnemyScripts.Count == 0)
        {
            // ���� ����� �� ����������� � ��� ����� ��������
            if (!isInfiniteMode && currentWave >= totalWaves)
            {
                OnAllWavesCleared?.Invoke();
                return;
            }

            // ������� ����� �����
            horde.SpawnEnemy();
            currentWave++;
            OnWaveStart?.Invoke(currentWave);
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
        yield return StartCoroutine(WaitForTurn(alchemancer.PlayerCombat));

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
