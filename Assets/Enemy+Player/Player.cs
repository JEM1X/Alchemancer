using UnityEngine;

public class Player : Combatant
{
    [SerializeField] private GameManager gameManager;

    
    protected override void Death()
    {
        base.Death();
        //������ ���������� ����
    }

    //private void Attack() 
    //{
    //    if (Input.GetKeyDown("KeyCode.Alpha1") ) 
    //    {
    //        gameManager.Enemies[0].TakeDamage(2);
    //        Debug.Log("�� ��������� �����");
    //    }

    //    GameManager.Instance.EndPlayerTurn();
    //}


}
