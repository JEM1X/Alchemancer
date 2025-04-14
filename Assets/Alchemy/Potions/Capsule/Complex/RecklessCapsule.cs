using UnityEngine;

[CreateAssetMenu(fileName = "RecklessCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/RecklessCapsule")]
public class RecklessCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.TakeDamage(user.PlayerCombat.Power * 3);

        user.PlayerCombat.InflictStun(1);

        enemy.StartCoroutine(enemy.AttackImpact());
    }
}
