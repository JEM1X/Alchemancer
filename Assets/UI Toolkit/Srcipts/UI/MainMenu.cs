using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement startMenu;
    private bool isStartVisible = true;


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

        var name = UITK.AddElement<Label>(canvas, "name");
        name.text = "Alchemancer";

        var menu = UITK.AddElement(canvas, "menu");

        var startButton = UITK.AddElement<Button>(menu, "startButton", "MainButton");
        startButton.text = "Начать игру";
        startButton.clicked += () => AudioM.Instance.PlaySound(AudioM.Instance.uiSounds[0]);
        startButton.clicked += ToggleStart;

        //var optionsButton = UITK.AddElement<Button>(menu, "optionsButton", "MainButton");
        //optionsButton.text = "Настройки";

        var exitButton = UITK.AddElement<Button>(menu, "exitButton", "MainButton");
        exitButton.text = "Выход";
        exitButton.clicked += () => AudioM.Instance.PlaySound(AudioM.Instance.uiSounds[0]);
        exitButton.clicked += Application.Quit;

        startMenu = UITK.AddElement(menu, "InGameFrame", "startMenu");
        startMenu.style.top = -100;

        var tutorialButton = UITK.AddElement<Button>(startMenu, "tutorialButton", "MainButton");
        tutorialButton.text = "Обучение";
        tutorialButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            SceneManager.LoadScene(4);
        };

        var runButton = UITK.AddElement<Button>(startMenu, "runButton", "MainButton");
        runButton.text = "Начать забег";
        runButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            SceneManager.LoadScene(1);
        };

        ToggleStart();
    }

    private void ToggleStart()
    {
        if (isStartVisible)
        {
            startMenu.style.display = DisplayStyle.None;
            isStartVisible = false;
        }
        else
        {
            startMenu.style.display = DisplayStyle.Flex;
            isStartVisible = true;
        }
    }
}