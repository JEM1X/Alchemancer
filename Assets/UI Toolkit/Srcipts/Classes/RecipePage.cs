using UnityEngine;
using UnityEngine.UIElements;

public class RecipePage
{
    public Potion_SO potion;
    public VisualElement page;
    public PotionCard potionCard;
    public bool isCovered = true;

    private VisualElement[] ingredientIcons = new VisualElement[3];
    private Label pageCover;


    public RecipePage(Potion_SO potion)
    {
        this.potion = potion;
        InitializeRecipePage();
        
        if (GameManager.Instance.discoveredPotions.Contains(potion))
        {
            UncoverPage();
            return;
        }

        GameManager.Instance.OnDiscoveredPotion += (Potion_SO discoveredPotion) =>
        {
            if (discoveredPotion != potion) return;
            UncoverPage();
        };
    }

    private void InitializeRecipePage()
    {
        page = UITK.CreateElement("page");

        potionCard = new PotionCard(potion);
        potionCard.cardFrame.pickingMode = PickingMode.Ignore;
        page.Add(potionCard.cardFrame);

        var ingredientPanel = UITK.AddElement(page, "ingredientPanel");

        for (int i = 0; i < potionCard.potion.Ingredients.Length; i++)
        {
            Ingredient_SO ingredient = potionCard.potion.Ingredients[i];
            ingredientIcons[i] = UITK.AddElement(ingredientPanel, "ingredientIcon");
            ingredientIcons[i].style.backgroundImage = new StyleBackground(ingredient.Icon);
        }

        pageCover = UITK.AddElement<Label>(page, "pageCover");
        pageCover.text = "Неизвестно";
    }

    public void UncoverPage()
    {
        pageCover.style.display = DisplayStyle.None;
        isCovered = false;
    }
}
