using UnityEngine;
using System.Linq;

public abstract class Potion_SO : Card_SO
{
    public Ingredient_SO[] Ingredients { get => ingredients; set => ingredients = value; }
    [SerializeField] private Ingredient_SO[] ingredients;
    

    public bool IsinRecipe(params Ingredient_SO[] ingredients)
    {
        if (ingredients.Length != this.ingredients.Length) return false;
        if (ingredients.Distinct().Count() != ingredients.Length) return false;

        foreach(var ingredient in ingredients)
        {
            if (!this.ingredients.Contains(ingredient))
                return false;
        }

        return true;
    }
} 
