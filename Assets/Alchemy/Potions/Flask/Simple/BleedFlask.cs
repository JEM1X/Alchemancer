using UnityEngine;

[CreateAssetMenu(fileName = "BleedFlask", menuName = "Scriptable Objects/Potion/Flask/BleedFlask")]
public class BleedFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictBleed(user.PlayerCombat.Influence / 2);
        }
    }
}
