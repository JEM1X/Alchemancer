using UnityEngine;

[CreateAssetMenu(fileName = "MultiCapsule", menuName = "Scriptable Objects/Potion/Capsule/MultiCapsule")]
public class MultiCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictWeakStrong(-1);
        enemy.InflictBleed(1);
        enemy.InflictVulnerableResilient(-1);
        enemy.InflictStun(1);
    }
}
