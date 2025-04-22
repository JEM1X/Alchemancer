using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Localization;

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
        startButton.clicked += () => audioSource.PlayOneShot(audioLibraire.uiSounds[0]);
        startButton.clicked += () => UIMenu.ToggleScreen(startMenu, ref isStartVisible);
        LTK.LocalizeStringUITK(startButton, LTK.UITABLE, "MainMenu.Start");

        var optionsButton = UITK.AddElement<Button>(menu, "optionsButton", "MainButton");
        optionsButton.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);
        LTK.LocalizeStringUITK(optionsButton, LTK.UITABLE, "MainMenu.Settings");

        var exitButton = UITK.AddElement<Button>(menu, "exitButton", "MainButton");
        exitButton.clicked += () => audioSource.PlayOneShot(audioLibraire.uiSounds[0]);
        exitButton.clicked += Application.Quit;
        LTK.LocalizeStringUITK(exitButton, LTK.UITABLE, "MainMenu.Quit");

        InitStartMenu();

        UIMenu.ToggleScreen(startMenu, ref isStartVisible);

        settingsScreen = UIMenu.InitSettingsMenu(audioMixer, out Button saveSettings);
        settingsScreen.style.alignSelf = Align.Center;
        settingsScreen.style.top = -70;
        menu.Add(settingsScreen);

        UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        saveSettings.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);
    }

    private void InitStartMenu()
    {
        startMenu = UITK.AddElement(menu, "InGameFrame", "startMenu");
        startMenu.style.top = -100;

        var tutorialButton = UITK.AddElement<Button>(startMenu, "tutorialButton", "MainButton");
        tutorialButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(5);
        };
        var tutorialButtonText = new LocalizedString("UI", "MainMenu.Tutorial");
        tutorialButtonText.StringChanged += (value) => tutorialButton.text = value;


        var runButton = UITK.AddElement<Button>(startMenu, "runButton", "MainButton");
        runButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(1);
        };
        var runButtonText = new LocalizedString("UI", "MainMenu.Run");
        runButtonText.StringChanged += (value) => runButton.text = value;

        var IGMButton = UITK.AddElement<Button>(startMenu, "IGMButton", "MainButton");
        IGMButton.clicked += () =>
        {
            GameManager.Instance.GenerateRecipes();
            GameManager.Instance.totalScore = 0;
            SceneManager.LoadScene(6);
        };
        var IGMButtonText = new LocalizedString("UI", "MainMenu.IGM");
        IGMButtonText.StringChanged += (value) => IGMButton.text = value;
    }
}