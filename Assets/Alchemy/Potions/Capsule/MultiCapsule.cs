using UnityEngine;

[CreateAssetMenu(fileName = "MultiCapsule", menuName = "Scriptable Objects/Potion/Capsule/MultiCapsule")]
public class MultiCapsule : Capsule_SO
{
    [SerializeField] private int amount = 1;


    public override void UseCapsule(Enemy enemy)
    {
        enemy.InflictWeakStrong(-amount);
        enemy.InflictBleed(amount);
        enemy.InflictVulnerableResilient(-amount);
    }
}
