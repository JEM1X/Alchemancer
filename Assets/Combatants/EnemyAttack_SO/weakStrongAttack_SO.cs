using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/WeakStrongAttack")]
public class WeakStrongAttack_SO : EnemyAttack_SO
{
    [SerializeField] private int weakStrongAmount;
    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Player.InflictWeakStrong(weakStrongAmount);
    }
}
