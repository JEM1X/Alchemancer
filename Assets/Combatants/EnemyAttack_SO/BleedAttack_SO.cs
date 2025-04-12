using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/BleedAttack")]
public class BleedAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int bleedAmount;

    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictBleed(bleedAmount);
    }
}
