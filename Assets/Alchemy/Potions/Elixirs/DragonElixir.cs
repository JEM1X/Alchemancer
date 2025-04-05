using UnityEngine;

[CreateAssetMenu(fileName = "DragonElixir", menuName = "Scriptable Objects/Potion/Elixir/DragonElixir")]
public class DragonElixir : Elixir_SO
{
    [SerializeField] private int amountHP = 2;
    [SerializeField] private int amountRes = 1;


    public override void UseElixir(AlchemancerMediator mediator)
    {
        mediator.PlayerCombat.InflictVulnerableResilient(amountRes);
        mediator.PlayerCombat.TakeHeal(amountHP);
    }
}
