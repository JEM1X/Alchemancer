using UnityEngine;

public class PlayerCombat : Combatant
{
    [SerializeField] private BattleManager battleManager;


    //protected override void Death()
    //{
    //    base.Death();
    //    //������ ���������� ����
    //}
    protected virtual void OnDestroy()
    {
        ClearAllListeners();
    }



}
