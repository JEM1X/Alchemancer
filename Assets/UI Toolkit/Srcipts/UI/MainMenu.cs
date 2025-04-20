using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLibraire audioLibraire;
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement menu;
    private VisualElement startMenu;
    private bool isStartVisible = true;
    private VisualElement settingsScreen;
    private bool isSettingsVisible = true;


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

        menu = UITK.AddElement(canvas, "menu");

        var startButton = UITK.AddElement<Button>(menu, "startButton", "MainButton");
        startButton.text = "Начать игру";
        startButton.clicked += () => audioSource.PlayOneShot(audioLibraire.uiSounds[0]);
        startButton.clicked += () => UIMenu.ToggleScreen(startMenu, ref isStartVisible);

        var optionsButton = UITK.AddElement<Button>(menu, "optionsButton", "MainButton");
        optionsButton.text = "Настройки";
        optionsButton.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        settingsScreen = UIMenu.InitSettingsMenu(audioMixer, out Button saveSettings);
        settingsScreen.style.alignSelf = Align.Center;
        settingsScreen.style.top = -70;
        menu.Add(settingsScreen);

        UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        saveSettings.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        var exitButton = UITK.AddElement<Button>(menu, "exitButton", "MainButton");
        exitButton.text = "Выход";
        exitButton.clicked += () => audioSource.PlayOneShot(audioLibraire.uiSounds[0]);
        exitButton.clicked += Application.Quit;

        InitStartMenu();

        UIMenu.ToggleScreen(startMenu, ref isStartVisible);
    }

    private void InitStartMenu()
    {
        startMenu = UITK.AddElement(menu, "InGameFrame", "startMenu");
        startMenu.style.top = -100;

        var tutorialButton = UITK.AddElement<Button>(startMenu, "tutorialButton", "MainButton");
        tutorialButton.text = "Обучение";
        tutorialButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(5);
        };

        var runButton = UITK.AddElement<Button>(startMenu, "runButton", "MainButton");
        runButton.text = "Начать забег";
        runButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(1);
        };

        var IGMButton = UITK.AddElement<Button>(startMenu, "IGMButton", "MainButton");
        IGMButton.text = "Бесконечный забег";
        IGMButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(6);
        };
    }
}