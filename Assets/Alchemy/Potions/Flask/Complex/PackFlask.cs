using UnityEngine;

[CreateAssetMenu(fileName = "PackFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/PackFlask")]
public class PackFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        int damage = enemies.Length;

        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(damage);

            enemy.StartCoroutine(enemy.AttackImpact());
        }
    }
}
