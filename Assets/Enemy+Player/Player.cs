using UnityEngine;

public class Player : Combatant
{
    [SerializeField] private int health = 10;
    [SerializeField] private GameManager _gameManager;
    private void Awake()// ��������� ����� ��?
    {
        Health = health;
    }
    
    protected override void Death()
    {
        base.Death();
        //������ ���������� ����
    }
    //private void Attack() 
    //{
    //    if (Input.GetKeyDown("KeyCode.Alpha1") ) 
    //    {
    //        _gameManager.Enemies[0].TakeDamage(2);
    //        Debug.Log("�� ��������� �����");
    //    }

    //    GameManager.Instance.EndPlayerTurn();
    //}


}
