using UnityEngine;
using System;
using System.Collections.Generic;

public class Horde : MonoBehaviour
{
    public List<Enemy> EnemyScript { get => enemyScripts; }
    [SerializeField] private List<Enemy> enemyScripts;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int spawnCount;

    public event Action<Enemy> OnNewEnemy;


    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation, transform);

            if(!enemy.TryGetComponent<Enemy>(out Enemy script))
            {
                Debug.LogError("Enemy Script was not found");
                return;
            }

            enemyScripts.Add(script);

            OnNewEnemy?.Invoke(script);
        }
    }
}
