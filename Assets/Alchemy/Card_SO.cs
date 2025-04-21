using UnityEngine;

public abstract class Card_SO : ScriptableObject
{
    public string Label { get => label; }
    [SerializeField] private string label = "Название";
    public Sprite Icon { get => icon; }
    [SerializeField] private Sprite icon;
    public string Description { get => description; }
    [SerializeField] private string description = "Описание";
}
