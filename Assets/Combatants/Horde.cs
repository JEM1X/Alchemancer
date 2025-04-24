using UnityEngine;
using System;
using System.Collections.Generic;

public class Horde : MonoBehaviour
{
    [Header("Horde")]
    public List<Enemy> EnemyScripts { get => enemyScripts; }
    [SerializeField] protected List<Enemy> enemyScripts;

    [SerializeField] protected GameObject[] enemyPrefabs;
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected int spawnCount;

    public event Action<Enemy> OnNewEnemy;
    public event Action OnNoEnemyLeft;

    public virtual void SpawnNewWave()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)],
                spawnPoints[i].position, spawnPoints[i].rotation, transform);

            if (!enemy.TryGetComponent<Enemy>(out Enemy script))
            {
                Debug.LogError("Enemy Script was not found");
                return;
            }

            enemyScripts.Add(script);

            TriggerOnNewEnemy(script);
            script.OnDeath += () => UpdateEnemyList(script);
        }
    }

    protected void UpdateEnemyList(Enemy enemy)
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

    protected void TriggerOnNewEnemy(Enemy enemy)
    {
        OnNewEnemy?.Invoke(enemy);
    }
}
