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

    private AlchemancerMediator mediator;
    private int drawAmount = 6;
    private int potionMaxAmount = 3;

    public event Action<Ingredient_SO> OnNewIngredient;
    public event Action<Potion_SO> OnNewPotion;


    private void Awake()
    {
        mediator = GetComponent<AlchemancerMediator>();
        mediator.PlayerCombat.OnAttackStart += DrawNewHand;
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
            Ingredient_SO newIngredient = ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)];
            playerIngredients.Add(newIngredient);
            OnNewIngredient?.Invoke(newIngredient);
        }
    }

    public void BrewNewPotion(Ingredient_SO[] ingredients)
    {
        if (playerPotions.Count >= potionMaxAmount) return;

        if (!ingredients.All(playerIngredients.Contains))
        {
            Debug.LogWarning("No ingredients available");
        }

        foreach (var ingredient in ingredients)
            playerIngredients.Remove(ingredient);

        if (!TryCombineIngredients(ingredients, out Potion_SO craftedPotion))
        {
            Debug.Log("Incorrect recipe");
            return;
        }

        playerPotions.Add(craftedPotion);
        OnNewPotion?.Invoke(craftedPotion);

        if (GameManager.Instance.discoveredPotions.Contains(craftedPotion)) return;
        GameManager.Instance.UnlockPotion(craftedPotion);
    }

    public bool TryCombineIngredients(Ingredient_SO[] ingredients, out Potion_SO craftedPotion)
    {
        int uniqueness = ingredients.Distinct().Count();

        if (uniqueness == 1)
        {
            craftedPotion = potionList.AllPotions[UnityEngine.Random.Range(0, potionList.AllPotions.Length)];
            return true;
        }

        if (ingredients.Distinct().Count() != ingredients.Length)
        {
            craftedPotion = null;
            return false;
        }

        craftedPotion = potionList.AllPotions.FirstOrDefault(potion =>
        potion.Ingredients.Length == ingredients.Length && ingredients.All(potion.IsinRecipe));

        return craftedPotion != null;
    }

    public void UseElixir(Elixir_SO elixir)
    {
        if (playerPotions.Remove(elixir))
        {
            elixir.UseElixir(mediator);
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
            flask.UseFlask(mediator.Horde.EnemyScripts.ToArray());
            return;
        }

        Debug.Log("No potion available");
    }
}
