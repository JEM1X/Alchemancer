using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/VulnerableFlask")]
public class VulnerableFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictVulnerableResilient(-user.PlayerCombat.Influence / 2);
        }
    }
}
