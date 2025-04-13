using UnityEngine;

[CreateAssetMenu(fileName = "PoisonFlask", menuName = "Scriptable Objects/Potion/Flask/Simple/PoisonFlask")]
public class PoisonFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].TakeDamage(user.PlayerCombat.Power / 2);
        }
    }
}
