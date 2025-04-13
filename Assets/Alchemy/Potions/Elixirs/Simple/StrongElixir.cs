using UnityEngine;

[CreateAssetMenu(fileName = "StrongElixir", menuName = "Scriptable Objects/Potion/Elixir/StrongElixir")]
public class StrongElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictWeakStrong(user.PlayerCombat.Influence);
    }
}
