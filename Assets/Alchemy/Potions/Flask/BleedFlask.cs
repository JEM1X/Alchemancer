using UnityEngine;

[CreateAssetMenu(fileName = "BleedFlask", menuName = "Scriptable Objects/Potion/Flask/BleedFlask")]
public class BleedFlask : Flask_SO
{
    [SerializeField] private int _rounds = 3;


    public override void UseFlask(Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictBleed(_rounds);
        }
    }
}
