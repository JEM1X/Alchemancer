using UnityEngine;

[CreateAssetMenu(fileName = "PackFlask", menuName = "Scriptable Objects/Potion/Flask/PackFlask")]
public class PackFlask : Flask_SO
{
    public override void UseFlask(Enemy[] enemies)
    {
        int damage = enemies.Length;

        foreach (Enemy enemy in enemies)
            enemy.TakeDamage(damage);
    }
}
