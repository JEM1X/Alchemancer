using UnityEngine;

public class Player : Combatant
{
    [SerializeField] private int health = 10;
    [SerializeField] private GameManager _gameManager;
    private void Awake()// поставить старт мб?
    {
        Health = health;
    }
    
    protected override void Death()
    {
        base.Death();
        //Логика завершения игры
    }
    //private void Attack() 
    //{
    //    if (Input.GetKeyDown("KeyCode.Alpha1") ) 
    //    {
    //        _gameManager.Enemies[0].TakeDamage(2);
    //        Debug.Log("Вы атаковали врага");
    //    }

    //    GameManager.Instance.EndPlayerTurn();
    //}


}
