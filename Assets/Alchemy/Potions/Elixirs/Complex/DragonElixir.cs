using UnityEngine;

[CreateAssetMenu(fileName = "DragonElixir", menuName = "Scriptable Objects/Potion/Elixir/Complex/DragonElixir")]
public class DragonElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.TakeHeal(user.PlayerCombat.Influence * 2);
        user.PlayerCombat.InflictVulnerableResilient(1);
    }
}
