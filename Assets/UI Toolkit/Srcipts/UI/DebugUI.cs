using UnityEngine;
using UnityEngine.UIElements;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private Alchemancer alchemancer;
    [SerializeField] private PotionList_SO potionList;

    private VisualElement canvas;


    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        canvas = UITK.AddElement<ScrollView>(root, "canvas");
        canvas.style.height = 900;
        canvas.style.width = 800;
        canvas.style.alignItems = Align.FlexStart;
        canvas.style.paddingTop = 10;
        canvas.style.paddingLeft = 10;
        ToggleDebug();

        foreach(Potion_SO potion in potionList.SimplePotions)
        {
            var potionButton = UITK.AddElement<Button>(canvas);
            potionButton.text = "Добавить " + potion.Label;
            potionButton.clicked += () => AddPotion(potion);
        }

        foreach (Potion_SO potion in potionList.ComplexPotions)
        {
            var potionButton = UITK.AddElement<Button>(canvas);
            potionButton.text = "Добавить " + potion.Label;
            potionButton.clicked += () => AddPotion(potion);
        }
    }

    private void AddPotion(Potion_SO potion)
    {
        foreach (Ingredient_SO ingredient in potion.Ingredients)
        {
            alchemancer.PlayerHand.DrawIngredient(ingredient);
        }

        alchemancer.PlayerHand.BrewNewPotion(potion.Ingredients);
    }

    public void ToggleDebug()
    {
        UITK.ToggleScreen(canvas, out _);
    }
}
