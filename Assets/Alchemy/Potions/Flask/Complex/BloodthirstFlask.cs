using UnityEngine;

[CreateAssetMenu(fileName = "BloodthirstFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/BloodthirstFlask")]
public class BloodthirstFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Bleed <= 0) continue;

            enemy.TakeDamage(enemy.Bleed);

            enemy.StartCoroutine(enemy.AttackImpact());
        }
    }
}
