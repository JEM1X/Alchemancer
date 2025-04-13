using UnityEngine;

[CreateAssetMenu(fileName = "GiantCapsule", menuName = "Scriptable Objects/Potion/Capsule/GiantCapsule")]
public class GiantCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        int damage = Mathf.Min(user.PlayerCombat.Power * 2, enemy.Health / 2);
        enemy.TakeDamage(damage);
    }
}
