using UnityEngine;

public class PlayerCombat : Combatant
{


    //protected override void Death()
    //{
    //    base.Death();
    //    //Логика завершения игры
    //}
    protected virtual void OnDestroy()
    {
        ClearAllListeners();
    }



}
