using UnityEngine;

[CreateAssetMenu(fileName = "InflictDull", menuName = "Scriptable Objects/Enemy/Action/InflictDull")]
public class InflictDullAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Alchemancer.PlayerCombat.InflictDullBright(-user.Influence);
    }
}
