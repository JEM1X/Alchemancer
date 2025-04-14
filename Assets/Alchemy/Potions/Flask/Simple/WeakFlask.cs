using UnityEngine;

[CreateAssetMenu(fileName = "WeakFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/WeakFlask")]
public class WeakFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.InflictWeakStrong(-user.PlayerCombat.Influence / 2);

            enemy.StartCoroutine(enemy.CastImpact());
        }
    }
}
