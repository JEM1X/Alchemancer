using UnityEngine;

[CreateAssetMenu(fileName = "RecipeList_SO", menuName = "Scriptable Objects/Alchemy/RecipeList_SO")]
public class RecipeList_SO : ScriptableObject
{
    public Potion_SO[] PotionRecipes { get => potionRecipes; }
    [SerializeField] private Potion_SO[] potionRecipes;
}
