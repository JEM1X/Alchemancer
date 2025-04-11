using UnityEngine;

[CreateAssetMenu(fileName = "VampireElixir", menuName = "Scriptable Objects/Potion/Elixir/VampireElixir")]
public class VampireElixir : Elixir_SO
{
    public override void UseElixir(AlchemancerMediator mediator)
    {
        int heal = 0;
        foreach(Enemy enemy in BattleM.Instance.Horde.EnemyScripts)
        {
            if (enemy.Bleed <= 0) continue;

            heal += enemy.Bleed;
        }
        
        mediator.PlayerCombat.TakeHeal(heal);
    }
}
