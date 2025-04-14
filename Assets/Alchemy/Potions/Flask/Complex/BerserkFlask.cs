using UnityEngine;

[CreateAssetMenu(fileName = "BerserkFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/BerserkFlask")]
public class BerserkFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        int lostHealth = user.PlayerCombat.HealthMax - user.PlayerCombat.Health;
        int damage = Mathf.Min(user.PlayerCombat.Power * 2, lostHealth / enemies.Length);

        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(damage);
        }
    }
}
