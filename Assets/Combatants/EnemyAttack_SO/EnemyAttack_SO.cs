using UnityEngine;

public abstract class EnemyAttack_SO : ScriptableObject
{
    public Sprite attackIcon;

    public abstract void ExecuteAttack(int Damage /*Enemy enemy = null*/);
    
}
