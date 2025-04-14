using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/VulnerableFlask")]
public class VulnerableFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.InflictVulnerableResilient(-user.PlayerCombat.Influence / 2);

            enemy.StartCoroutine(enemy.CastImpact());
        }
    }
}
