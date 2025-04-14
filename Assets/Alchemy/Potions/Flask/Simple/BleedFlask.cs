using UnityEngine;

[CreateAssetMenu(fileName = "BleedFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/BleedFlask")]
public class BleedFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.InflictBleed(user.PlayerCombat.Power / 2);

            enemy.StartCoroutine(enemy.AttackImpact());
        }
    }
}
