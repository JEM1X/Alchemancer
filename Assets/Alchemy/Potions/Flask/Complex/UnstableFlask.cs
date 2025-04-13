using UnityEngine;

[CreateAssetMenu(fileName = "UnstableFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/UnstableFlask")]
public class UnstableFlask : Flask_SO
{
    [SerializeField] private int amount = 2;

    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            int randomI = Random.Range(0, 4);

            switch (randomI)
            {
                case 0:
                    enemy.InflictStun(amount);
                    break;
                case 1:
                    enemy.InflictDullBright(-amount);
                    break;
                case 2:
                    enemy.InflictVulnerableResilient(-amount);
                    break;
                case 3:
                    enemy.InflictWeakStrong(-amount);
                    break;
            }
        }
    }
}
