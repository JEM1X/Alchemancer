using UnityEngine;

[CreateAssetMenu(fileName = "PoisonFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/PoisonFlask")]
public class PoisonFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(user.PlayerCombat.Power / 2);

            enemy.StartCoroutine(enemy.AttackImpact());
        }
    }
}
