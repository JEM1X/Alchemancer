using UnityEngine;

[CreateAssetMenu(fileName = "InflictWeak", menuName = "Scriptable Objects/Enemy/Action/InflictWeak")]
public class InflictWeakAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Mediator.PlayerCombat.InflictWeakStrong(-user.Influence);
    }
}
