using UnityEngine;

[CreateAssetMenu(fileName = "GamblerFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/GamblerFlask")]
public class GamblerFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        int coinFlip = Random.Range(0, 2);

        switch (coinFlip)
        {
            case 0:
                user.PlayerCombat.TakeTrueDamage(user.PlayerCombat.Health);
                break;
            case 1:
                foreach (Enemy enemy in enemies)
                {
                    enemy.TakeTrueDamage(enemy.Health);
                }
                break;
        }
    }
}
