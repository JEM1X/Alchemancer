using UnityEngine;

public class IngredientCard : UICard
{
    public Ingredient_SO ingredient;
    public bool isInHand = true;

    public IngredientCard(Ingredient_SO ingredient)
    {
        this.ingredient = ingredient;
        InitializeCard(ingredient);
    }
}
