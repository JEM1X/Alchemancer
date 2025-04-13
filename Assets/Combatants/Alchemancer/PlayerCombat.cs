using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : Combatant
{
    public Action CompletePlayerTurn;


    protected override IEnumerator Action()
    {
        Debug.Log("Player Attack");
        bool isTurnCompleted = false;
        CompletePlayerTurn = () => isTurnCompleted = true;

        yield return new WaitUntil(() => isTurnCompleted);
    }
}
