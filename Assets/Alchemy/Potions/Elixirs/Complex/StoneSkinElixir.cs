using UnityEngine;

[CreateAssetMenu(fileName = "StoneSkinElixir", menuName = "Scriptable Objects/Potion/Elixir/Complex/StoneSkinElixir")]
public class StoneSkinElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictVulnerableResilient(user.PlayerCombat.Influence);
        user.PlayerCombat.InflictWeakStrong(-1);
    }
}
