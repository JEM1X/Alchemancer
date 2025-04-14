using UnityEngine;

[CreateAssetMenu(fileName = "FullStopFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/FullStopFlask")]
public class FullStopFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.InflictStun(2);

            enemy.StartCoroutine(enemy.CastImpact());
        }

        user.PlayerCombat.InflictStun(2);
    }
}
