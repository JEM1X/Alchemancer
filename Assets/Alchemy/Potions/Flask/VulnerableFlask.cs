using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableFlask", menuName = "Scriptable Objects/Potion/Flask/VulnerableFlask")]
public class VulnerableFlask : Flask_SO
{
    [SerializeField] private int amount = 2;


    public override void UseFlask(Enemy[] enemies)
    {
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].InflictVulnerableResilient(-amount);
        }
    }
}
