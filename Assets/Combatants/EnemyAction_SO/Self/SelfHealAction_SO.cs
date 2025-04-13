using UnityEngine;

[CreateAssetMenu(fileName = "SelfHeal", menuName = "Scriptable Objects/Enemy/Action/SelfHeal")]
public class SelfHealAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.TakeHeal(user.Influence * 2);
    }
}
