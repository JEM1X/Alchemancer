using UnityEngine;

[CreateAssetMenu(fileName = "DragonElixir", menuName = "Scriptable Objects/Potion/Elixir/DragonElixir")]
public class DragonElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictVulnerableResilient(user.PlayerCombat.Influence / 2);
        user.PlayerCombat.TakeHeal(user.PlayerCombat.Influence);
    }
}
