using UnityEngine;

[CreateAssetMenu(fileName = "CharlatanElixir", menuName = "Scriptable Objects/Potion/Elixir/Complex/CharlatanElixir")]
public class CharlatanElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        int randomHeal = Random.Range(0, user.PlayerCombat.Influence * 4 + 1);
        user.PlayerCombat.TakeHeal(randomHeal);
    }
}
