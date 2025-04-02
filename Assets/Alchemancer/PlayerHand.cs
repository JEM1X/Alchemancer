using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private List<Ingredient_SO> playerIngredients;
    [SerializeField] private List<Potion_SO> playerPotions = new(0);

    private AlchemancerMediator mediator;
    private int drawAmount = 6;
    private int potionMaxAmount = 3;

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
        if (playerPotions.Count >= potionMaxAmount) return;

        for (int i = 0; i < ingredients.Length; i++)
        {
            if(playerIngredients.Remove(ingredients[i]))
                continue;
            
            Debug.Log("No ingredients available");
            return;
        }

        if (!mediator.Cauldron.TryCombineIngredients(ingredients, out Potion_SO craftedPotion))
        {
            Debug.Log("Incorrect recipe");
            return;
        }

        playerPotions.Add(craftedPotion);

        OnPotionChange?.Invoke(playerPotions.ToArray());
    }

    public void UseCapsule(Capsule_SO capsule, Enemy enemy)
    {
        if (playerPotions.Remove(capsule))
        {
            capsule.UseCapsule(enemy);
            return;
        }

        Debug.Log("No potion available");
    }
}
