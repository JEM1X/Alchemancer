using UnityEngine;

[CreateAssetMenu(fileName = "BleedCapsule", menuName = "Scriptable Objects/Potion/Capsule/BleedCapsule")]
public class BleedCapsule : Capsule_SO
{
    [SerializeField] private int _rounds = 3;


    public override void UseCapsule(Enemy enemy)
    {
        enemy.InflictBleed(_rounds);
    }
}
