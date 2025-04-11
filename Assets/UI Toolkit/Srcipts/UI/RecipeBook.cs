using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class RecipeBook : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private PotionList_SO potionList;
    [SerializeField] private Sprite sprite;

    private VisualElement background;
    private List<RecipePage> potionPages = new();
    private int currentPage = 1;
    private bool isVisible = true;


    private void Start()
    {
        InitializeUI();
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

        foreach (Potion_SO potion  in potionList.AllPotions)
        {
            RecipePage recipePage = new RecipePage(potion);
            var potionPage = recipePage.page;
            bookFrame.Add(potionPage);
            potionPages.Add(recipePage);
        }

        TurnPage(currentPage);

        var nextPage = UITK.AddElement<Button>(bookFrame, "nextPage");
        nextPage.text = "Следующая страница";
        nextPage.clicked += () => TurnPage(currentPage + 2);

        var previousPage = UITK.AddElement<Button>(bookFrame, "previousPage");
        previousPage.text = "Предыдущая страница";
        previousPage.clicked += () => TurnPage(currentPage - 2);

        var toggleButton = UITK.AddElement<Button>(canvas, "toggleButton");
        toggleButton.style.backgroundImage = new StyleBackground(sprite);
        toggleButton.clicked += () => ToggleGuide();

        ToggleGuide(false);
    }

    private void TurnPage(int page)
    {
        if (page <= 0) return;
        if (page >= potionPages.Count) return;

        foreach (RecipePage recipePage in potionPages)
        {
            recipePage.page.style.display = DisplayStyle.None;
        }

        potionPages[page - 1].page.style.display = DisplayStyle.Flex;
        potionPages[page].page.style.display = DisplayStyle.Flex;

        currentPage = page;
    }

    private void ToggleGuide(bool sound = true)
    {
        if (isVisible)
        {
            background.style.display = DisplayStyle.None;
            isVisible = false;

            if(sound)
                AudioM.Instance.PlaySound(AudioM.Instance.guideSounds[1]);
        }
        else
        {
            background.style.display = DisplayStyle.Flex;
            isVisible = true;
            if(sound)
                AudioM.Instance.PlaySound(AudioM.Instance.guideSounds[0]);
        }
    }
}
