using UnityEngine;

[CreateAssetMenu(fileName = "DullFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/DullFlask")]
public class DullFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictDullBright(user.PlayerCombat.Influence / 2);
        }
    }
}
