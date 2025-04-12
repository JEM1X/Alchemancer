using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/StunAttack")]
public class StunAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int stunAmount;

    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictStun(stunAmount);
    }
}
