using UnityEngine;

[CreateAssetMenu(fileName = "AcidCapsule", menuName = "Scriptable Objects/Potion/Capsule/AcidCapsule")]
public class AcidCapsule : Capsule_SO
{
    [SerializeField] private int damage = 3;


    public override void UseCapsule(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }
}
