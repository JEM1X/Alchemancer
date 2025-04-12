using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/BasicAttack")]
public class BasicAttack_SO : EnemyAttack_SO
{
    public override void ExecuteAttack(int Damage)
    {
        BattleM.Instance.Player.TakeDamage(Damage);
    }
}
