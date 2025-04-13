using UnityEngine;

[CreateAssetMenu(fileName = "SimpleAttack", menuName = "Scriptable Objects/Enemy/Action/SimpleAttack")]
public class SimpleAttack_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.AttackLunge());
        BattleM.Instance.Alchemancer.PlayerCombat.TakeDamage(user.Power);
    }
}
