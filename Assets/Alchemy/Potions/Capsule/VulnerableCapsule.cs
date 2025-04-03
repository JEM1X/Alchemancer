using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableCapsule", menuName = "Scriptable Objects/Potion/Capsule/VulnerableCapsule")]
public class VulnerableCapsule : Capsule_SO
{
    [SerializeField] private int amount = 3;


    public override void UseCapsule(Enemy enemy)
    {
        enemy.InflictVulnerableResilient(-amount);
    }
}
