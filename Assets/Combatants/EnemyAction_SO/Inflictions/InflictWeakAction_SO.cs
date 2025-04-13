using UnityEngine;

[CreateAssetMenu(fileName = "InflictWeak", menuName = "Scriptable Objects/Enemy/Action/InflictWeak")]
public class InflictWeakAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.AttackLunge());
        BattleM.Instance.Alchemancer.PlayerCombat.InflictWeakStrong(-user.Influence);
    }
}
