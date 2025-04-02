using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private List<Ingredient_SO> playerIngredients;
    [SerializeField] private Potion_SO[] playerPotions = new Potion_SO[3];

    private AlchemancerMediator mediator;
    private int drawAmount = 6;

    public event Action<Ingredient_SO[]> OnHandChange;
    public event Action<Potion_SO[]> OnPotionChange;


    private void Awake()
    {
        mediator = GetComponent<AlchemancerMediator>();
    }

    public void DrawNewHand()
    {
        playerIngredients.Clear();

        for (int i = 0; i < drawAmount; i++) 
        {
            playerIngredients.Add(ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)]);
        }

        OnHandChange?.Invoke(playerIngredients.ToArray());
    }

    public void CraftNewPotion(Ingredient_SO[] ingredients)
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            if(playerIngredients.Remove(ingredients[i]))
                continue;
            
            Debug.Log("No ingredients available");
            return;
        }

        if (!mediator.Cauldron.TryCombineIngredients(ingredients, out Potion_SO potion))
        {
            Debug.Log("Incorrect recipe");
            return;
        }
        
        for (int i = 0; i < playerPotions.Length; i++)
        {
            if (playerPotions[i] != null) continue;

            playerPotions[i] = potion;

            OnPotionChange?.Invoke(playerPotions);

            break;
        }
    }
}
