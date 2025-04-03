using UnityEngine;

[CreateAssetMenu(fileName = "WeakFlask", menuName = "Scriptable Objects/Potion/Flask/WeakFlask")]
public class WeakFlask : Flask_SO
{
    [SerializeField] private int amount = 2;


    public override void UseFlask(Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictWeakStrong(-amount);
        }
    }
}
