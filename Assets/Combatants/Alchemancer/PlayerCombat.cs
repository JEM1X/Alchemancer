using UnityEngine;

public class PlayerCombat : Combatant
{


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
