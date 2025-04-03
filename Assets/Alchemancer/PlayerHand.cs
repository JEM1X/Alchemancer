using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private PotionList_SO RecipeList;
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
        DrawCards(drawAmount);
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerIngredients.Add(ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)]);
        }

        OnHandChange?.Invoke(playerIngredients.ToArray());
    }

    public void CraftNewPotion(Ingredient_SO[] ingredients)
    {
        if (playerPotions.Count >= potionMaxAmount) return;

        if (!ingredients.All(playerIngredients.Contains))
        {
            Debug.LogWarning("No ingredients available");
            return;
        }

        foreach (var ingredient in ingredients)
            playerIngredients.Remove(ingredient);

        if (!TryCombineIngredients(ingredients, out Potion_SO craftedPotion))
        {
            Debug.Log("Incorrect recipe");
            return;
        }

        playerPotions.Add(craftedPotion);
        OnPotionChange?.Invoke(playerPotions.ToArray());
    }

    public bool TryCombineIngredients(Ingredient_SO[] ingredients, out Potion_SO craftedPotion)
    {
        craftedPotion = RecipeList.AllPotions.FirstOrDefault(potion =>
        potion.Ingredients.Length == ingredients.Length && ingredients.All(potion.IsinRecipe));

        return craftedPotion != null;
    }

    public void UseElixir(Elixir_SO elixir)
    {
        if (playerPotions.Remove(elixir))
        {
            elixir.UseElixir(mediator.Player);
            return;
        }

        Debug.Log("No potion available");
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

    public void UseFlask(Flask_SO flask)
    {
        if (playerPotions.Remove(flask))
        {
            flask.UseFlask(mediator.Horde.EnemyScript.ToArray());
            return;
        }

        Debug.Log("No potion available");
    }
}
