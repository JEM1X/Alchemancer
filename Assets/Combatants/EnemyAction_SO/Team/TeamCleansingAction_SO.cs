using UnityEngine;

[CreateAssetMenu(fileName = "TeamCleansing", menuName = "Scriptable Objects/Enemy/Action/TeamCleansing")]
public class TeamCleansingAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        foreach (var enemy in BattleM.Instance.Horde.EnemyScripts)
        {
            enemy.InflictBleed(-enemy.Bleed);
            enemy.InflictStun(-enemy.Stun);
            enemy.InflictVulnerableResilient(-enemy.VulnerableResilient);
            enemy.InflictWeakStrong(-enemy.WeakStrong);
        }
    }
}
