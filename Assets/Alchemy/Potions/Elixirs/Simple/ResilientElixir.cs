using UnityEngine;

[CreateAssetMenu(fileName = "ResilientElixir", menuName = "Scriptable Objects/Potion/Elixir/Simple/ResilientElixir")]
public class ResilientElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictVulnerableResilient(user.PlayerCombat.Influence);
    }
}
