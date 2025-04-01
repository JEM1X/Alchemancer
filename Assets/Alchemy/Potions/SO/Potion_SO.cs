using UnityEngine;
using System.Linq;

public abstract class Potion_SO : Card_SO
{
    public Ingredient_SO[] Ingredients { get => ingredients; }
    [SerializeField] private Ingredient_SO[] ingredients;
    

    public bool IsinRecipe(Ingredient_SO ingredient)
    {
        if (ingredients.Contains(ingredient))
            return true;

        return false;
    }
} 
