using UnityEngine;

[CreateAssetMenu(fileName = "DullFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/DullFlask")]
public class DullFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.InflictDullBright(-user.PlayerCombat.Influence / 2);

            enemy.StartCoroutine(enemy.CastImpact());
        }
    }
}
