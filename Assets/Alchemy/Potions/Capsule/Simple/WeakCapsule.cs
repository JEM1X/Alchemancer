using UnityEngine;

[CreateAssetMenu(fileName = "WeakCapsule", menuName = "Scriptable Objects/Potion/Capsule/Simple/WeakCapsule")]
public class WeakCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictWeakStrong(-user.PlayerCombat.Influence);

        enemy.StartCoroutine(enemy.CastImpact());
    }
}
