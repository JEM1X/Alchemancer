using UnityEngine;

[CreateAssetMenu(fileName = "SimpleAttack", menuName = "Scriptable Objects/Enemy/Action/SimpleAttack")]
public class SimpleAttack_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Alchemancer.PlayerCombat.TakeDamage(user.Power);
    }
}
