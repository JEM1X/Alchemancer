using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/weakStrongAttack")]
public class weakStrongAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int weakStrongAmount;
    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Player.InflictWeakStrong(weakStrongAmount);
    }
}
