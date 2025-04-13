using UnityEngine;

public abstract class EnemyAction_SO : ScriptableObject
{
    public Sprite actionIcon;


    public abstract void ExecuteAction(Enemy user);
}
