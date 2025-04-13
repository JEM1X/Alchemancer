using UnityEngine;

[CreateAssetMenu(fileName = "FindingElixir", menuName = "Scriptable Objects/Potion/Elixir/FindingElixir")]
public class FindingElixir : Elixir_SO
{
    [SerializeField] private int drawAmount = 4;


    public override void UseElixir(Alchemancer user)
    {
        user.PlayerHand.DrawCards(drawAmount);
    }
}
