using UnityEngine;
using System;
using System.Collections.Generic;

public class Horde : MonoBehaviour
{
    public List<Enemy> EnemyScripts { get => enemyScripts; }
    [SerializeField] private List<Enemy> enemyScripts;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int spawnCount;

    public event Action<Enemy> OnNewEnemy;
    public event Action OnNoEnemyLeft;

    [Header("Infinite Mode Settings")]
    [SerializeField] private int maxEnemyTypes = 3; // ћаксимальное количество типов врагов
    [SerializeField] private int wavesPerNewEnemyType = 1; // Ќовый тип врага каждые N волн

    public void SpawnEnemy()
    {
        if (BattleM.Instance.IsInfiniteMode) 
        {
            InfinityGamemode();
            //вместо обращени€ к инстансу можно просто параметр к методу добавить и все
        }
        else 
        {
            ClassicGamemode();
        }
        
    }

    private void UpdateEnemyList(Enemy enemy)
    {
        if (!enemyScripts.Remove(enemy))
        {
            Debug.LogWarning("No enemy found");
            return;
        }

        if (enemyScripts.Count > 0) return;

        Debug.Log("No enemy left");
        OnNoEnemyLeft?.Invoke();
    }

    private void ClassicGamemode() 
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], spawnPoints[i].position, spawnPoints[i].rotation, transform);

            if (!enemy.TryGetComponent<Enemy>(out Enemy script))
            {
                Debug.LogError("Enemy Script was not found");
                return;
            }

            enemyScripts.Add(script);

            OnNewEnemy?.Invoke(script);
            script.OnDeath += () => UpdateEnemyList(script);
        }
    }

    private void InfinityGamemode()
    {
        int currentWave = BattleM.Instance.CurrentWave;     
        int unlockedEnemyTypes = 1 + currentWave / wavesPerNewEnemyType;
        int availableEnemyTypes = Mathf.Min(unlockedEnemyTypes, maxEnemyTypes, enemyPrefabs.Length);

        
        for (int i = 0; i < spawnCount; i++)
        {
            int enemyTypeIndex = UnityEngine.Random.Range(0, availableEnemyTypes); // ¬ыбираем случайный разблокированный тип

            GameObject enemy = Instantiate(
                enemyPrefabs[enemyTypeIndex],
                spawnPoints[i].position,
                spawnPoints[i].rotation,
                transform
            );

            if (!enemy.TryGetComponent(out Enemy script))
            {
                Debug.LogError("Enemy Script was not found");
                continue;
            }

            enemyScripts.Add(script);
            OnNewEnemy?.Invoke(script);
            script.OnDeath += () => UpdateEnemyList(script);
        }
    }

}
