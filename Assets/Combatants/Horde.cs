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


    private void Start()
    {
        //SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], spawnPoints[i].position, spawnPoints[i].rotation, transform);

            if(!enemy.TryGetComponent<Enemy>(out Enemy script))
            {
                Debug.LogError("Enemy Script was not found");
                return;
            }

            enemyScripts.Add(script);

            OnNewEnemy?.Invoke(script);
            script.OnDeath += () => UpdateEnemyList(script);

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


    
    public void RemoveEnemy(Enemy enemy)
    {
        EnemyScripts.Remove(enemy);
        if (EnemyScripts.Count == 0 || !EnemyScripts.Exists(e => e != null))
            OnNoEnemyLeft?.Invoke();
    }

}
