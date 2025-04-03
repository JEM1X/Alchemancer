using UnityEngine;

[CreateAssetMenu(fileName = "WeakCapsule", menuName = "Scriptable Objects/Potion/Capsule/WeakCapsule")]
public class WeakCapsule : Capsule_SO
{
    [SerializeField] private int amount = 3;


    public override void UseCapsule(Enemy enemy)
    {
        enemy.InflictWeakStrong(-amount);
    }
}
