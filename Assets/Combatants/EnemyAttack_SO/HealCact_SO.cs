using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy/Attacks/HealCast")]
public class HealCact_SO : EnemyAttack_SO
{
    [SerializeField] private int healAmount;
    public override void ExecuteAttack(int Damage)
    {
        
        foreach (var enemy in BattleM.Instance.Horde.EnemyScripts) 
        {
            enemy.TakeHeal(healAmount);
        }
    }
}
