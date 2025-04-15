using UnityEngine;
using UnityEngine.UIElements;

public class IngredientCard : UICard
{
    public Ingredient_SO ingredient;
    public bool isSelected = false;

    public IngredientCard(Ingredient_SO ingredient)
    {
        this.ingredient = ingredient;
        InitializeCard(ingredient);
    }

    public bool Select()
    {
        if (isSelected)
        {
            isSelected = false;
            cardFrame.style.scale = StyleKeyword.Null;
            cardFrame.style.translate = StyleKeyword.Null;

            return false;
        }
        else
        {
            isSelected = true;
            cardFrame.style.scale = new StyleScale(new Vector2(1.2f, 1.2f));
            cardFrame.style.translate = new StyleTranslate(new Translate(0, -80));

            return true;
        }
    }
}
