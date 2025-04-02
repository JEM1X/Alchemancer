using UnityEngine;
using System;

public class Enemy : Combatant
{
    
    [SerializeField] private int health = 5;
    private void Awake()
    {
        Health = health;    
    }
    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
    public void TakeTurn(System.Action onTurnEnd)
    {
        Debug.Log(name + " атакует игрока!");
        
        //GameManager.Instance.PlayerTakeDamage(2); // Условная атака
        onTurnEnd.Invoke(); // Передаем ход дальше
    }

}
