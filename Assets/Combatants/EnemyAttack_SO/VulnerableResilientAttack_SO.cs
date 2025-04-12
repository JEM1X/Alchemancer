using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/VulnerableResilientAttack")]

public class VulnerableResilientAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int ResilentAmount;
    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Player.InflictVulnerableResilient(ResilentAmount);
    }
}
