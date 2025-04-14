using UnityEngine;

public abstract class EnemyAction_SO : ScriptableObject
{
    public Sprite ActionIcon { get => actionIcon; }
    [SerializeField] private Sprite actionIcon;
    public Sprite SubIcon { get => subIcon; }
    [SerializeField] private Sprite subIcon;
    public string Description { get => description; }
    [TextArea] [SerializeField] private string description = "Описание";


    public abstract void ExecuteAction(Enemy user);
}
