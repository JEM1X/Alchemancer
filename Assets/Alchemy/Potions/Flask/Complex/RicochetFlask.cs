using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RicochetFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/RicochetFlask")]
public class RicochetFlask : Flask_SO
{
    [SerializeField] private float chance = 0.5f;

    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        var enemyList = enemies.ToList();

        while (true)
        {
            var chosenEnemy = enemyList[Random.Range(0, enemyList.Count)];

            chosenEnemy.TakeDamage(user.PlayerCombat.Power);

            if (chosenEnemy.Health <= 0)
                enemyList.Remove(chosenEnemy);

            chosenEnemy.StartCoroutine(chosenEnemy.AttackImpact());

            if (Random.Range(0f, 1f) < chance) break;
        }
    }
}
