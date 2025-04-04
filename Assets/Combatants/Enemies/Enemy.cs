using UnityEngine;
using System;

public class Enemy : Combatant
{
    
    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
    public void TakeTurn(System.Action onTurnEnd)
    {
        Debug.Log(name + " атакует игрока!");
        
        
        BM.Instance.PlayerTakeDamage(2); // Условная атака
        onTurnEnd.Invoke(); // Передаем ход дальше
    }
    public void UseFlask(System.Action onTurnEnd) 
    {
        Debug.Log(name + " использует зелье!");

        BM.Instance.PlayerTakeDamage(2); // Условная атака
        onTurnEnd.Invoke(); // Передаем ход дальше    

    }

}
