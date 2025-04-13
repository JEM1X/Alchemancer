using UnityEngine;

[CreateAssetMenu(fileName = "InflictStun", menuName = "Scriptable Objects/Enemy/Action/InflictStun")]
public class InflictStunAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictStun(user.Influence / 2);
    }
}
