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
        Debug.Log(name + " ������� ������!");
        
        
        BM.Instance.PlayerTakeDamage(2); // �������� �����
        onTurnEnd.Invoke(); // �������� ��� ������
    }
    public void UseFlask(System.Action onTurnEnd) 
    {
        Debug.Log(name + " ���������� �����!");

        BM.Instance.PlayerTakeDamage(2); // �������� �����
        onTurnEnd.Invoke(); // �������� ��� ������    

    }

}
