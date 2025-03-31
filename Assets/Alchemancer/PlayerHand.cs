using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private Ingredient_SO[] ingredients = new Ingredient_SO[6];

    public event Action<Ingredient_SO[]> OnDrawNewHand;


    private void Start()
    {
        DrawNewHand();
    }

    public void DrawNewHand()
    {
        for (int i = 0; i < ingredients.Length; i++) 
        {
            ingredients[i] = ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)];
        }

        OnDrawNewHand(ingredients);
    }
}
