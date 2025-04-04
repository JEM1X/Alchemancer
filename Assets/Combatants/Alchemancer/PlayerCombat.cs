using UnityEngine;

public class PlayerCombat : Combatant
{
    
    protected virtual void OnDestroy()
    {
        ClearAllListeners();
    }



}
