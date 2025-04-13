using UnityEngine;

[CreateAssetMenu(fileName = "DullCapsule", menuName = "Scriptable Objects/Potion/Capsule/DullCapsule")]
public class DullCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.InflictDullBright(-user.PlayerCombat.Influence);
    }
}
