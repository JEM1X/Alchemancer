using UnityEngine;

[CreateAssetMenu(fileName = "CleansingElixir", menuName = "Scriptable Objects/Potion/Elixir/Complex/CleansingElixir")]
public class CleansingElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictBleed(-user.PlayerCombat.Bleed);
        user.PlayerCombat.InflictStun(-user.PlayerCombat.Stun);
        user.PlayerCombat.InflictVulnerableResilient(-user.PlayerCombat.VulnerableResilient);
        user.PlayerCombat.InflictWeakStrong(-user.PlayerCombat.WeakStrong);
    }
}

