using UnityEngine;

[CreateAssetMenu(fileName = "RicochetFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/RicochetFlask")]
public class RicochetFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        while (true)
        {
            var chosenEnemy = enemies[Random.Range(0, enemies.Length)];
            chosenEnemy.TakeDamage(user.PlayerCombat.Power);

            if (Random.Range(0f, 1f) > 0.5) break;
        }
    }
}
