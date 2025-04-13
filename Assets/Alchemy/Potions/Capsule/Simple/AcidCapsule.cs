using UnityEngine;

[CreateAssetMenu(fileName = "AcidCapsule", menuName = "Scriptable Objects/Potion/Capsule/Simple/AcidCapsule")]
public class AcidCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.TakeDamage(user.PlayerCombat.Power);
    }
}
