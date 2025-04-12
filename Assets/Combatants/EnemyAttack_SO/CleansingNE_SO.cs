using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/antidebuff")]
public class CleansingNE_SO : EnemyAttack_SO
{
    public override void ExecuteAttack(int Damage)
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
