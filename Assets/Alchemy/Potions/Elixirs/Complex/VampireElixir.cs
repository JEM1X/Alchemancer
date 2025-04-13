using UnityEngine;

[CreateAssetMenu(fileName = "VampireElixir", menuName = "Scriptable Objects/Potion/Elixir/VampireElixir")]
public class VampireElixir : Elixir_SO
{
    public override void UseElixir(Alchemancer user)
    {
        int heal = 0;
        foreach(Enemy enemy in BattleM.Instance.Horde.EnemyScripts)
        {
            if (enemy.Bleed <= 0) continue;

            heal += enemy.Bleed;
        }
        
        user.PlayerCombat.TakeHeal(heal);
    }
}
