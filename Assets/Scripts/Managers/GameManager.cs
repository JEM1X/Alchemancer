using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int TotalScore;
    public List<Potion_SO> unlockedPotions;

    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private PotionList_SO potionList;
    [SerializeField] private AlchemancerMediator mediator;

    public event Action<Potion_SO> OnDiscoveredPotion;


    protected override void Awake()
    {
        base.Awake();
        Enemy.OnScoreGain += AddScore;
        DontDestroyOnLoad(gameObject);
        GenerateRecipes();
        mediator.PlayerHand.OnNewPotion += UnlockPotion;
    }

    private void AddScore(int _score) 
    {
        TotalScore += _score;    
    }


    void GenerateRecipes()
    {
        List<Ingredient_SO[]> allRecipes = new();

        int length = ingredientList.Ingredients.Length;
        for (int i = 0; i < length - 2; i++)
        {
            for (int j = i + 1; j < length - 1; j++)
            {
                for (int k = j + 1; k < length; k++)
                {
                    Ingredient_SO[] recipe =
                    {
                        ingredientList.Ingredients[i],
                        ingredientList.Ingredients[j],
                        ingredientList.Ingredients[k]
                    };

                    allRecipes.Add(recipe);
                }
            }
        }

        for (int i = 0; i < potionList.AllPotions.Length; i++)
        {
            int randomRecipe = UnityEngine.Random.Range(0, allRecipes.Count);
            potionList.AllPotions[i].Ingredients = allRecipes[randomRecipe];
            allRecipes.Remove(allRecipes[randomRecipe]);
        }
    }

    private void UnlockPotion(Potion_SO potion)
    {
        if (!unlockedPotions.Contains(potion)) return;

        unlockedPotions.Add(potion);
        OnDiscoveredPotion(potion);
    }
}
