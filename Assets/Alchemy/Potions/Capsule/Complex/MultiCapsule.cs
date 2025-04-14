using UnityEngine;

[CreateAssetMenu(fileName = "MultiCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/MultiCapsule")]
public class MultiCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictVulnerableResilient(-1);
        enemy.InflictWeakStrong(-1);
        enemy.InflictDullBright(-1);
        enemy.InflictBleed(1);
        enemy.InflictStun(1);

        enemy.StartCoroutine(enemy.CastImpact());
    }
}
