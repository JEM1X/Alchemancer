using UnityEngine;

[CreateAssetMenu(fileName = "SwapCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/SwapCapsule")]
public class SwapCapsule : Capsule_SO
{
    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        int vulRes = user.PlayerCombat.VulnerableResilient;
        int weakStrong = user.PlayerCombat.WeakStrong;
        int dullBright = user.PlayerCombat.DullBright;
        int bleed = user.PlayerCombat.Bleed;
        int stun = user.PlayerCombat.Stun;

        user.PlayerCombat.InflictVulnerableResilient(enemy.VulnerableResilient);
        user.PlayerCombat.InflictWeakStrong(enemy.WeakStrong);
        user.PlayerCombat.InflictDullBright(enemy.DullBright);
        user.PlayerCombat.InflictBleed(enemy.Bleed);
        user.PlayerCombat.InflictStun(enemy.Stun);

        enemy.InflictVulnerableResilient(vulRes);
        enemy.InflictWeakStrong(weakStrong);
        enemy.InflictDullBright(dullBright);
        enemy.InflictBleed(bleed);
        enemy.InflictStun(stun);
    }
}
