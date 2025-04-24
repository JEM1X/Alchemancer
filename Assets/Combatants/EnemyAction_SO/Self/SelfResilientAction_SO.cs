using UnityEngine;

[CreateAssetMenu(fileName = "SelfResilient", menuName = "Scriptable Objects/Enemy/Action/SelfResilient")]
public class SelfResilientAction_SO : EnemyAction_SO
{
    public override void ExecuteAction(Enemy user)
    {
        user.StartCoroutine(user.SelfCast());
        user.InflictVulnerableResilient(user.Influence);
    }
}
