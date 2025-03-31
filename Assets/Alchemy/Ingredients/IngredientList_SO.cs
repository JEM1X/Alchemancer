using UnityEngine;

[CreateAssetMenu(fileName = "IngredientList_SO", menuName = "Scriptable Objects/IngredientList_SO")]
public class IngredientList_SO : ScriptableObject
{
    public Ingredient_SO[] Ingredients { get => ingredients; }
    [SerializeField] private Ingredient_SO[] ingredients;
}
