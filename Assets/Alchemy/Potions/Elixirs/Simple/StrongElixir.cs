using UnityEngine;

[CreateAssetMenu(fileName = "StrongElixir", menuName = "Scriptable Objects/Potion/Elixir/Simple/StrongElixir")]
public class StrongElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictWeakStrong(user.PlayerCombat.Influence);
    }
}
