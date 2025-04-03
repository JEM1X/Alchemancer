using UnityEngine;

[CreateAssetMenu(fileName = "HealthElixir", menuName = "Scriptable Objects/Potion/Elixir/HealthElixir")]
public class HealthElixir : Elixir_SO
{
    [SerializeField] private int heal = 5;


    public override void UseElixir(AlchemancerMediator mediator)
    {
        mediator.Player.TakeHeal(heal);
    }
}
