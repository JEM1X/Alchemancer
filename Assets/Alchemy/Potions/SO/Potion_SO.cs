using UnityEngine;
using System.Linq;

public abstract class Potion_SO : Card_SO
{
    public Ingredient_SO[] Ingredients { get => ingredients; set => ingredients = value; }
    [SerializeField] private Ingredient_SO[] ingredients;
    

    public bool IsinRecipe(params Ingredient_SO[] ingredients)
    {
        foreach(var ingredient in ingredients)
        {
            if (!this.ingredients.Contains(ingredient))
                return false;
        }

        return true;
    }
} 
