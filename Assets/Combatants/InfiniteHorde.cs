using UnityEngine;

public class InfiniteHorde : Horde
{
    [Header("Infinite Mode Settings")]
    [SerializeField] private int wavesPerNewEnemyType = 1;
    [SerializeField] private int wavesPerStatIncrease = 5;
    [SerializeField] private int healthIncrease = 1;
    [SerializeField] private int damageIncrease = 1;

    public override void SpawnNewWave()
    {
        int currentWave = BattleM.Instance.CurrentWave;
        int unlockedEnemyTypes = 1 + currentWave / wavesPerNewEnemyType;
        int availableEnemyTypes = Mathf.Min(unlockedEnemyTypes, enemyPrefabs.Length);
        int statIncreaseCount = currentWave / wavesPerStatIncrease;

        for (int i = 0; i < spawnCount; i++)
        {
            int enemyTypeIndex = Random.Range(0, availableEnemyTypes);

            var enemy = Instantiate(
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

            if (statIncreaseCount > 0)
            {
                script.IncreaseStats(
                    healthIncrease * statIncreaseCount,
                    damageIncrease * statIncreaseCount
                );
            }

            enemyScripts.Add(script);
            TriggerOnNewEnemy(script);
            script.OnDeath += () => UpdateEnemyList(script);
        }
    }
}
