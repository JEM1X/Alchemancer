using UnityEngine;

[CreateAssetMenu(fileName = "UnstableFlask", menuName = "Scriptable Objects/Potion/Flask/Complex/UnstableFlask")]
public class UnstableFlask : Flask_SO
{
    public override void UseFlask(Alchemancer user, Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            int randomI = Random.Range(0, 4);

            switch (randomI)
            {
                case 0:
                    enemy.InflictStun(1);
                    break;
                case 1:
                    enemy.InflictDullBright(-1);
                    break;
                case 2:
                    enemy.InflictVulnerableResilient(-1);
                    break;
                case 3:
                    enemy.InflictWeakStrong(-1);
                    break;
            }
        }
    }
}
