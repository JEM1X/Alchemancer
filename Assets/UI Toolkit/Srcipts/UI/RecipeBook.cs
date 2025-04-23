using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class RecipeBook : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Alchemancer alchemancer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLibraire audioLibraire;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private Sprite sprite;
    [SerializeField] private PotionList_SO potionList;

    private VisualElement background;
    private List<RecipePage> potionPages = new();
    private List<RecipePage> availablePotionPages = new();
    private int currentPage = 0;
    private bool isAvailableMode = false;
    private bool isVisible = true;


    private void Start()
    {
        InitializeUI();
        PlayerHand.OnNewIngredient += (Ingredient_SO _) => UpdateAvailablePotions();
        PlayerHand.OnIngredientUse += (Ingredient_SO _) => UpdateAvailablePotions();
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        background = UITK.AddElement(canvas, "background");

        var bookFrame = UITK.AddElement(background, "bookFrame");

        foreach (Potion_SO potion  in potionList.SimplePotions)
        {
            RecipePage recipePage = new RecipePage(potion);
            var potionPage = recipePage.page;

            bookFrame.Add(potionPage);
            potionPages.Add(recipePage);

            recipePage.potionCard.cardFrame.clicked += () =>
            {
                if (recipePage.isCovered)
                {
                    return;
                }
                else
                {
                    Ingredient_SO[] ingredients = recipePage.potionCard.potion.Ingredients;
                    alchemancer.PlayerHand.BrewNewPotion(ingredients);

                    audioSource.PlayOneShot(audioLibraire.potionSounds[0]);
                }
            };
        }

        foreach (Potion_SO potion in potionList.ComplexPotions)
        {
            RecipePage recipePage = new RecipePage(potion);
            var potionPage = recipePage.page;
            bookFrame.Add(potionPage);
            potionPages.Add(recipePage);

            recipePage.potionCard.cardFrame.clicked += () =>
            {
                if (recipePage.isCovered)
                {
                    return;
                }
                else
                {
                    Ingredient_SO[] ingredients = recipePage.potionCard.potion.Ingredients;
                    alchemancer.PlayerHand.BrewNewPotion(ingredients);

                    audioSource.PlayOneShot(audioLibraire.potionSounds[0]);
                }
            };
        }

        TurnPage(currentPage);

        var nextPage = UITK.AddElement<Button>(bookFrame, "nextPage");
        LTK.LocalizeStringUITK(nextPage, LTK.UITABLE, "Combat.Recipe.NextPage");
        nextPage.clicked += () => TurnPage(currentPage + 2);

        var previousPage = UITK.AddElement<Button>(bookFrame, "previousPage");
        LTK.LocalizeStringUITK(previousPage, LTK.UITABLE, "Combat.Recipe.PreviousPage");
        previousPage.clicked += () => TurnPage(currentPage - 2);

        var availableToggle = UITK.AddElement<Button>(bookFrame, "availableToggle");
        LTK.LocalizeStringUITK(availableToggle, LTK.UITABLE, "Combat.Recipe.AvailableToggle");
        availableToggle.clicked += () =>
        {
            if(isAvailableMode)
                DeactivateAvailableMode();
            else
                ActivateAvailableMode();
        };

        var toggleButton = UITK.AddElement<Button>(canvas, "toggleButton");
        toggleButton.style.backgroundImage = new StyleBackground(sprite);
        toggleButton.clicked += () => ToggleGuide();

        ToggleGuide(false);
    }

    private void ActivateAvailableMode()
    {
        if (availablePotionPages.Count <= 0)
        {
            return;
        }

        isAvailableMode = true;
        TurnPage(0);
    }

    private void UpdateAvailablePotions()
    {
        availablePotionPages.Clear();

        foreach (RecipePage recipePage in potionPages)
        {
            var ingredients = recipePage.potion.Ingredients;

            if (recipePage.isCovered) continue;

            if (alchemancer.PlayerHand.CheckIngredients(ingredients))
                availablePotionPages.Add(recipePage);
        }

        if(availablePotionPages.Count <= 0) 
            DeactivateAvailableMode();
    }

    private void DeactivateAvailableMode()
    {
        isAvailableMode = false;
        TurnPage(0);
    }

    private void TurnPage(int page)
    {
        if (page < 0) return;

        List<RecipePage> currentPages = isAvailableMode ? availablePotionPages : potionPages;
        if (page >= currentPages.Count) return;

        HideAllPages(potionPages);
        HideAllPages(availablePotionPages);

        currentPages[page].page.style.display = DisplayStyle.Flex;

        if (page + 1 < currentPages.Count)
            currentPages[page + 1].page.style.display = DisplayStyle.Flex;

        currentPage = page;
    }

    private static void HideAllPages(List<RecipePage> pages)
    {
        foreach (RecipePage recipePage in pages)
        {
            recipePage.page.style.display = DisplayStyle.None;
        }
    }

    private void ToggleGuide(bool sound = true)
    {
        if (isVisible)
        {
            background.style.display = DisplayStyle.None;
            isVisible = false;

            if(sound)
                audioSource.PlayOneShot(audioLibraire.guideSounds[1]);
        }
        else
        {
            background.style.display = DisplayStyle.Flex;
            isVisible = true;

            if(sound)
                audioSource.PlayOneShot(audioLibraire.guideSounds[0]);
        }
    }
}
