using UnityEngine;

[CreateAssetMenu(fileName = "DragonElixir", menuName = "Scriptable Objects/Potion/Elixir/DragonElixir")]
public class DragonElixir : Elixir_SO
{
    [SerializeField] private int amount = 4;


    public override void UseElixir(AlchemancerMediator mediator)
    {
        mediator.PlayerCombat.InflictVulnerableResilient(amount);
        mediator.PlayerCombat.TakeHeal(amount);
    }
}
