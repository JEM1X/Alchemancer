using UnityEngine;

[CreateAssetMenu(fileName = "GiantCapsule", menuName = "Scriptable Objects/Potion/Capsule/GiantCapsule")]
public class GiantCapsule : Capsule_SO
{
    [SerializeField] private int maxDamage = 8;


    public override void UseCapsule(Enemy enemy)
    {
        int damage = Mathf.Min(maxDamage, enemy.Health / 2);
        enemy.TakeDamage(damage);
    }
}
