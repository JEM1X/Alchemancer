using UnityEngine;

[CreateAssetMenu(fileName = "BleedAttack", menuName = "Scriptable Objects/Enemy/Action/BleedAttack")]
public class BleedAttack_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.AttackLunge());
        BattleM.Instance.Alchemancer.PlayerCombat.InflictBleed(user.Power);
    }
}
