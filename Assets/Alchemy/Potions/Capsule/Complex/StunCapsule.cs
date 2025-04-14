using UnityEngine;

[CreateAssetMenu(fileName = "StunCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/StunCapsule")]
public class StunCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictStun(user.PlayerCombat.Influence / 2);

        enemy.StartCoroutine(enemy.CastImpact());
    }
}
