using UnityEngine;

[CreateAssetMenu(fileName = "PoisonFlask", menuName = "Scriptable Objects/Potion/Flask/PoisonFlask")]
public class PoisonFlask : Flask_SO
{
    [SerializeField] private int damage = 2;


    public override void UseFlask(Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].TakeDamage(damage);
        }
    }
}
