using UnityEngine;

[CreateAssetMenu(fileName = "FindingElixir", menuName = "Scriptable Objects/Potion/Elixir/Complex/FindingElixir")]
public class FindingElixir : Elixir_SO
{
    [SerializeField] private int drawAmount = 4;


    public override void UseElixir(Alchemancer user)
    {
        for (int i = 0; i < drawAmount; i++)
        {
            user.PlayerHand.DrawRandomIngredient();
        }
    }
}
