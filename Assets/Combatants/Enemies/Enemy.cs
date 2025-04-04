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
        
        BattleManager.Instance.PlayerTakeDamage(2); // Условная атака
        onTurnEnd.Invoke(); // Передаем ход дальше
    }

}
