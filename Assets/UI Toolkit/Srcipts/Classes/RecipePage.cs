using UnityEngine;
using UnityEngine.UIElements;

public class RecipePage
{
    public Potion_SO potion;
    public VisualElement page;
    public VisualElement[] ingredientIcons = new VisualElement[3];

    public RecipePage(Potion_SO potion)
    {
        this.potion = potion;
        InitializeRecipePage();
    }

    private void InitializeRecipePage()
    {
        page = UITK.CreateElement("page");

        PotionCard potionCard = new PotionCard(potion);
        potionCard.cardFrame.pickingMode = PickingMode.Ignore;
        page.Add(potionCard.cardFrame);

        var ingredientPanel = UITK.AddElement(page, "ingredientPanel");

        for (int i = 0; i < potionCard.potion.Ingredients.Length; i++)
        {
            Ingredient_SO ingredient = potionCard.potion.Ingredients[i];
            ingredientIcons[i] = UITK.AddElement(ingredientPanel, "ingredientIcon");
            ingredientIcons[i].style.backgroundImage = new StyleBackground(ingredient.Icon);
        }
    }
}
