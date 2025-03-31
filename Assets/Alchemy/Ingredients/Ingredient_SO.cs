using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/ingredient_SO")]
public class Ingredient_SO : ScriptableObject
{
    public string Label { get => label; }
    [SerializeField] private string label;
    public Sprite Icon { get => icon; }
    [SerializeField] private Sprite icon;
    public string Description { get => description; }
    [SerializeField] private string description; 
}
