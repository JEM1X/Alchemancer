using UnityEngine;

[CreateAssetMenu(fileName = "BrightElixir", menuName = "Scriptable Objects/Potion/Elixir/Simple/BrightElixir")]
public class BrightElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.InflictDullBright(user.PlayerCombat.Influence);
    }
}
