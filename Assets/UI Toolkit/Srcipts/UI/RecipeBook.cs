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
    [SerializeField] private PotionList_SO potionList;
    [SerializeField] private Sprite bookIcon;
    [SerializeField] private Sprite forwardIcon;
    [SerializeField] private Sprite markIcon;

    private VisualElement background;
    private List<RecipePage> potionPages = new();
    private List<RecipePage> availablePotionPages = new();
    private int currentPage = 0;
    private bool isAvailableMode = false;


    private void Start()
    {
        InitializeUI();
        PlayerHand.OnNewIngredient += UpdateAvailablePotionsEvt;
        PlayerHand.OnIngredientUse += UpdateAvailablePotionsEvt;
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

                    if(alchemancer.PlayerHand.BrewNewPotion(ingredients))
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

        TurnPage(currentPage, false);

        var nextPage = UITK.AddElement<Button>(bookFrame, "nextPage", "ClearButton");
        nextPage.style.backgroundImage = new StyleBackground(forwardIcon);
        nextPage.clicked += () => TurnPage(currentPage + 2);

        var previousPage = UITK.AddElement<Button>(bookFrame, "previousPage", "ClearButton");
        previousPage.style.backgroundImage = new StyleBackground(forwardIcon);
        previousPage.clicked += () => TurnPage(currentPage - 2);

        var availableToggle = UITK.AddElement<Button>(bookFrame, "availableToggle", "ClearButton");
        availableToggle.style.backgroundImage = new StyleBackground(markIcon);
        UITK.LocalizeStringUITK(availableToggle, UITK.UITABLE, "Combat.Recipe.AvailableToggle");
        availableToggle.clicked += () =>
        {
            if(isAvailableMode)
                DeactivateAvailableMode();
            else
                ActivateAvailableMode();
        };

        var toggleButton = UITK.AddElement<Button>(canvas, "toggleButton");
        toggleButton.style.backgroundImage = new StyleBackground(bookIcon);
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

    private void UpdateAvailablePotionsEvt(Ingredient_SO _)
    {
        UpdateAvailablePotions();
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

        if (availablePotionPages.Count <= 0)
            DeactivateAvailableMode();
        else
            TurnPage(0);
    }

    private void DeactivateAvailableMode()
    {
        isAvailableMode = false;
        TurnPage(0);
    }

    private void TurnPage(int page, bool sound = true)
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

        if (sound)
            PlayTurnPage();
    }

    private static void HideAllPages(List<RecipePage> pages)
    {
        foreach (RecipePage recipePage in pages)
        {
            recipePage.page.style.display = DisplayStyle.None;
        }
    }

    public void ToggleGuide(bool sound = true)
    {
        if (sound)
            UITK.ToggleScreenWSound(background, audioSource, audioLibraire.guideSounds[0], audioLibraire.guideSounds[1]);
        else
            UITK.ToggleScreen(background, out _);
    }

    private void PlayTurnPage()
    {
        if(background.resolvedStyle.display == DisplayStyle.Flex)
            audioSource.PlayOneShot(audioLibraire.guideSounds[2]);

    }

    private void OnDestroy()
    {
        PlayerHand.OnNewIngredient -= UpdateAvailablePotionsEvt;
        PlayerHand.OnIngredientUse -= UpdateAvailablePotionsEvt;
    }
}
