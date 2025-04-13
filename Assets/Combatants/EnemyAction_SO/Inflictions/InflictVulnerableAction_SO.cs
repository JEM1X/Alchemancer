using UnityEngine;

[CreateAssetMenu(fileName = "InflictVulnerable", menuName = "Scriptable Objects/Enemy/Action/InflictVulnerable")]
public class InflictVulnerableAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        BattleM.Instance.Alchemancer.PlayerCombat.InflictVulnerableResilient(-user.Influence);
    }
}
