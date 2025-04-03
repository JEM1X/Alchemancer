using UnityEngine;

[CreateAssetMenu(fileName = "ResilientElixir", menuName = "Scriptable Objects/Potion/Elixir/ResilientElixir")]
public class ResilientElixir : Elixir_SO
{
    [SerializeField] private int amount = 2;


    public override void UseElixir(AlchemancerMediator mediator)
    {
        mediator.Player.InflictVulnerableResilient(amount);
    }
}
