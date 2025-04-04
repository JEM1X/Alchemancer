using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private AlchemancerMediator mediator;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement victoryScreen;
    private bool isVictoryVisible = true;


    private void Awake()
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

        victoryScreen = UITK.AddElement(canvas, "victoryScreen");

        var victoryFrame = UITK.AddElement(victoryScreen, "victoryFrame");

        var victoryLabel = UITK.AddElement<Label>(victoryFrame, "victoryLabel");
        victoryLabel.text = "Победа";

        var nextButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
        nextButton.text = "Дальше";
        nextButton.clicked += () => SceneManager.LoadScene(1);

        ToggleVictoryScreen();
        //mediator.Horde.OnNoEnemyLeft += ToggleVictoryScreen;
    }

    private void ToggleVictoryScreen()
    {
        if (isVictoryVisible)
        {
            victoryScreen.style.display = DisplayStyle.None;
            isVictoryVisible = false;
        }
        else
        {
            victoryScreen.style.display = DisplayStyle.Flex;
            isVictoryVisible = true;
        }
    }
}
