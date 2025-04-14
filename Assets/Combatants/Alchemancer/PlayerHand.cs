using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private PotionList_SO potionList;
    public List<Ingredient_SO> PlayerIngredients { get => playerIngredients; }
    [SerializeField] private List<Ingredient_SO> playerIngredients;
    public List<Potion_SO> PlayerPotions { get => playerPotions; }
    [SerializeField] private List<Potion_SO> playerPotions;

    private Alchemancer alchemancer;
    private int drawAmount = 5;
    private int potionMaxAmount = 3;

    public event Action<Ingredient_SO> OnNewIngredient;
    public event Action<Ingredient_SO> OnIngredientUse;
    public event Action<Potion_SO> OnNewPotion;
    //public event Action<Potion_SO> OnPotionUse;


    private void Awake()
    {
        alchemancer = GetComponent<Alchemancer>();
        alchemancer.PlayerCombat.OnActionStart += DrawNewHand;
    }

    public void DrawNewHand()
    {
        playerIngredients.Clear();

        for (int i = 0; i < drawAmount; i++)
            DrawRandomIngredient();
    }

    public void DrawRandomIngredient()
    {
        DrawIngredient(ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)]);
    }

    public void DrawIngredient(Ingredient_SO newIngredient)
    {
        playerIngredients.Add(newIngredient);
        OnNewIngredient?.Invoke(newIngredient);
    }

    private void UseIngredient(Ingredient_SO ingredient)
    {
        playerIngredients.Remove(ingredient);
        OnIngredientUse?.Invoke(ingredient);
    } 

    public void BrewNewPotion(Ingredient_SO[] ingredients)
    {
        if (playerPotions.Count >= potionMaxAmount) return;

        if (!ingredients.All(playerIngredients.Contains))
        {
            Debug.LogWarning("No ingredients available");
            return;
        }

        foreach (var ingredient in ingredients)
            UseIngredient(ingredient);

        Potion_SO craftedPotion = null;
        if(TryBrewSimplePotion(ingredients, out Potion_SO craftedSimplePotion))
        {
            craftedPotion = craftedSimplePotion;
        }
        else if (TryBrewComplexPotion(ingredients, out Potion_SO craftedComplexPotion))
        {
            craftedPotion = craftedComplexPotion;
        }
        else
        {
            Debug.Log("Incorrect recipe");
            return;
        }

        playerPotions.Add(craftedPotion);
        OnNewPotion?.Invoke(craftedPotion);

        if (GameManager.Instance.discoveredPotions.Contains(craftedPotion)) return;

        GameManager.Instance.UnlockPotion(craftedPotion);
    }

    public bool TryBrewSimplePotion(Ingredient_SO[] ingredients, out Potion_SO craftedPotion)
    {
        craftedPotion = null;

        if (ingredients.Length != 2) return false;

        int uniqueness = ingredients.Distinct().Count();
        if (uniqueness == 1)
        {
            craftedPotion = potionList.SimplePotions[UnityEngine.Random.Range(0, potionList.SimplePotions.Length)];
            return true;
        }

        if (uniqueness != ingredients.Length) return false;

        craftedPotion = potionList.SimplePotions.FirstOrDefault(potion => ingredients.All(potion.IsinRecipe));

        return craftedPotion != null;
    }

    public bool TryBrewComplexPotion(Ingredient_SO[] ingredients, out Potion_SO craftedPotion)
    {
        craftedPotion = null;

        if (ingredients.Length != 3) return false;

        int uniqueness = ingredients.Distinct().Count();
        if (uniqueness == 1 )
        {
            craftedPotion = potionList.ComplexPotions[UnityEngine.Random.Range(0, potionList.ComplexPotions.Length)];
            return true;
        }

        if (uniqueness != ingredients.Length) return false;

        craftedPotion = potionList.ComplexPotions.FirstOrDefault(potion => ingredients.All(potion.IsinRecipe));

        return craftedPotion != null;
    }

    public void UseElixir(Elixir_SO elixir)
    {
        if (playerPotions.Remove(elixir))
        {
            elixir.UseElixir(alchemancer);
            return;
        }

        Debug.Log("No potion available");
    }

    public void UseCapsule(Capsule_SO capsule, Enemy enemy)
    {
        if (playerPotions.Remove(capsule))
        {
            capsule.UseCapsule(alchemancer, enemy);
            return;
        }

        Debug.Log("No potion available");
    }

    public void UseFlask(Flask_SO flask)
    {
        if (playerPotions.Remove(flask))
        {
            flask.UseFlask(alchemancer, BattleM.Instance.Horde.EnemyScripts.ToArray());
            return;
        }

        Debug.Log("No potion available");
    }
}
