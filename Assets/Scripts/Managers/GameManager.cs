using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int totalScore;
    public List<Potion_SO> discoveredPotions = new();

    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private PotionList_SO potionList;

    public event Action<Potion_SO> OnDiscoveredPotion;


    protected override void Awake()
    {
        base.Awake();
        Enemy.OnScoreGain += AddScore;
        DontDestroyOnLoad(gameObject);
    }

    private void AddScore(int _score) 
    {
        totalScore += _score;    
    }

    public void GenerateRecipes()
    {
        GenerateSimpleRecipes();

        GenerateComplexRecipes();

        discoveredPotions = new();
    }

    private void GenerateSimpleRecipes()
    {
        List<Ingredient_SO[]> newRecipes = new();

        int length = ingredientList.Ingredients.Length;
        for (int i = 0; i < length - 1; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                Ingredient_SO[] recipe =
                {
                    ingredientList.Ingredients[i],
                    ingredientList.Ingredients[j]
                };

                newRecipes.Add(recipe);
            }
        }

        for (int i = 0; i < potionList.SimplePotions.Length; i++)
        {
            int randomRecipe = UnityEngine.Random.Range(0, newRecipes.Count);
            potionList.SimplePotions[i].Ingredients = newRecipes[randomRecipe];
            newRecipes.Remove(newRecipes[randomRecipe]);
        }
    }

    private void GenerateComplexRecipes()
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

        for (int i = 0; i < potionList.ComplexPotions.Length; i++)
        {
            int randomRecipe = UnityEngine.Random.Range(0, allRecipes.Count);
            potionList.ComplexPotions[i].Ingredients = allRecipes[randomRecipe];
            allRecipes.Remove(allRecipes[randomRecipe]);
        }
    }

    public void UnlockPotion(Potion_SO potion)
    {
        discoveredPotions.Add(potion);
        OnDiscoveredPotion(potion);
    }
}
