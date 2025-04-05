using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;


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
        startButton.clicked += () => SceneManager.LoadScene(1);

        //var optionsButton = UITK.AddElement<Button>(menu, "optionsButton", "MainButton");
        //optionsButton.text = "Настройки";

        var exitButton = UITK.AddElement<Button>(menu, "exitButton", "MainButton");
        exitButton.text = "Выход";
        exitButton.clicked += Application.Quit;
    }
}