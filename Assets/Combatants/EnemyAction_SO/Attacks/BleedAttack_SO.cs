using UnityEngine;

[CreateAssetMenu(fileName = "BleedAttack", menuName = "Scriptable Objects/Enemy/Action/BleedAttack")]
public class BleedAttack_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictBleed(user.Power);
    }
}
