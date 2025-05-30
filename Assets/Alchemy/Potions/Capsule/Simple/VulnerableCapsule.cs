using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableCapsule", menuName = "Scriptable Objects/Potion/Capsule/Simple/VulnerableCapsule")]
public class VulnerableCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictVulnerableResilient(-user.PlayerCombat.Influence);

        enemy.StartCoroutine(enemy.CastImpact());
    }
}
