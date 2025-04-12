using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/VulnerableResilientAttack")]

public class VulnerableResilientAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int ResilientAmount;
    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictVulnerableResilient(ResilientAmount);
    }
}
