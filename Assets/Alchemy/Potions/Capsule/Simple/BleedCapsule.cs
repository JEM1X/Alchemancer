using UnityEngine;

[CreateAssetMenu(fileName = "BleedCapsule", menuName = "Scriptable Objects/Potion/Capsule/BleedCapsule")]
public class BleedCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictBleed(user.PlayerCombat.Power);
    }
}
