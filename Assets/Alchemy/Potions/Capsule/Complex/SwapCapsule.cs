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

        user.PlayerCombat.InflictVulnerableResilient(-user.PlayerCombat.VulnerableResilient);
        user.PlayerCombat.InflictWeakStrong(-user.PlayerCombat.WeakStrong);
        user.PlayerCombat.InflictDullBright(-user.PlayerCombat.DullBright);
        user.PlayerCombat.InflictBleed(-user.PlayerCombat.Bleed);
        user.PlayerCombat.InflictStun(-user.PlayerCombat.Stun);

        user.PlayerCombat.InflictVulnerableResilient(enemy.VulnerableResilient);
        user.PlayerCombat.InflictWeakStrong(enemy.WeakStrong);
        user.PlayerCombat.InflictDullBright(enemy.DullBright);
        user.PlayerCombat.InflictBleed(enemy.Bleed);
        user.PlayerCombat.InflictStun(enemy.Stun);

        enemy.InflictVulnerableResilient(-enemy.VulnerableResilient);
        enemy.InflictWeakStrong(-enemy.WeakStrong);
        enemy.InflictDullBright(-enemy.DullBright);
        enemy.InflictBleed(-enemy.Bleed);
        enemy.InflictStun(-enemy.Stun);

        enemy.InflictVulnerableResilient(vulRes);
        enemy.InflictWeakStrong(weakStrong);
        enemy.InflictDullBright(dullBright);
        enemy.InflictBleed(bleed);
        enemy.InflictStun(stun);
    }
}
