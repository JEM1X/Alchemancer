using UnityEngine;

[CreateAssetMenu(fileName = "ComboCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/ComboCapsule")]
public class ComboCapsule : Capsule_SO
{
    [SerializeField] private int attacksAmount;

    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        for (int i = 0; i < attacksAmount; i++)
        {
            enemy.TakeDamage(user.PlayerCombat.Power / 2);

            if (enemy == null) return;
        }
    }
}
