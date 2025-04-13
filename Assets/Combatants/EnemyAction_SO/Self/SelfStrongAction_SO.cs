using UnityEngine;

[CreateAssetMenu(fileName = "SelfStrong", menuName = "Scriptable Objects/Enemy/Action/SelfStrong")]
public class SelfStrongAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.InflictWeakStrong(user.Influence);
    }
}
