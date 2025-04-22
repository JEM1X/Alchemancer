using UnityEngine;

[CreateAssetMenu(fileName = "DodgeElixir", menuName = "Scriptable Objects/Potion/Elixir/Simple/DodgeElixir")]
public class DodgeElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictDodge(user.PlayerCombat.Influence / 2);
    }
}
