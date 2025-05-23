using UnityEngine;

[CreateAssetMenu(fileName = "InflictStun", menuName = "Scriptable Objects/Enemy/Action/InflictStun")]
public class InflictStunAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.AttackLunge());
        BattleM.Instance.Alchemancer.PlayerCombat.InflictStun(user.Influence / 2);
    }
}
