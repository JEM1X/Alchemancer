using UnityEngine;

[CreateAssetMenu(fileName = "SelfBleedHeal", menuName = "Scriptable Objects/Enemy/Action/SelfBleedHeal")]
public class SelfBleedHealAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.AttackLunge());
        user.TakeHeal(BattleM.Instance.Alchemancer.PlayerCombat.Bleed);
    }
}
