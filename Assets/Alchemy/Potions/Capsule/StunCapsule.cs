using UnityEngine;

[CreateAssetMenu(fileName = "StunCapsule", menuName = "Scriptable Objects/Potion/Capsule/StunCapsule")]
public class StunCapsule : Capsule_SO
{
    [SerializeField] private int amount = 1;

    public override void UseCapsule(Enemy enemy)
    {
        enemy.InflictStun(amount);
    }
}
