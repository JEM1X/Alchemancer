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
    private VisualElement defeatScreen;
    private bool isDefeatVisible = true;

    private void Awake()
    {
        InitializeUI();
    }

    private void Start()
    {
        BattleM.Instance.OnAllWavesCleared += ToggleVictoryScreen;
        BattleM.Instance.OnPlayerLose += ToggleDefeatScreen;
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
        nextButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        ToggleVictoryScreen();

        defeatScreen = UITK.AddElement(canvas, "defeatScreen");

        var defeatFrame = UITK.AddElement(defeatScreen, "defeatFrame");

        var defeatLabel = UITK.AddElement<Label>(defeatFrame, "defeatLabel");
        defeatLabel.text = "Поражение";

        var againButton = UITK.AddElement<Button>(defeatFrame, "againButton", "MainButton");
        againButton.text = "Начать занаво";
        againButton.clicked += () => SceneManager.LoadScene(1);

        ToggleDefeatScreen();
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

    private void ToggleDefeatScreen()
    {
        if (isDefeatVisible)
        {
            defeatScreen.style.display = DisplayStyle.None;
            isDefeatVisible = false;
        }
        else
        {
            defeatScreen.style.display = DisplayStyle.Flex;
            isDefeatVisible = true;
        }
    }
}
