using UnityEngine;

[CreateAssetMenu(fileName = "HealthElixir", menuName = "Scriptable Objects/Potion/Elixir/HealthElixir")]
public class HealthElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        user.PlayerCombat.TakeHeal(user.PlayerCombat.Influence * 2);
    }
}
