using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Alchemancer alchemancer;
    [SerializeField] private string stageName;
    [SerializeField] private bool isFinal;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLibraire audioLibraire;
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement canvas;
    private VisualElement pauseScreen;
    private bool isPauseVisible = true;
    private VisualElement settingsScreen;
    private bool isSettingsVisible = true;
    private VisualElement victoryScreen;
    private bool isVictoryVisible = true;
    private VisualElement defeatScreen;
    private bool isDefeatVisible = true;
    private Label waveCounter;


    private void Start()
    {
        StartCoroutine(InitializeUI());
        BattleM.Instance.OnWaveStart += UpdateWaveCounter;
        BattleM.Instance.OnAllWavesCleared += () => UIMenu.ToggleScreen(victoryScreen, ref isVictoryVisible);
        alchemancer.PlayerCombat.OnDeath += () => UIMenu.ToggleScreen(defeatScreen, ref isDefeatVisible);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
        }
    }

    private IEnumerator InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        canvas = UITK.AddElement(root, "canvas", "MainText");

        InitPauseScreen();

        InitVictoryScreen();

        InitDefeatScreen();

        waveCounter = UITK.AddElement<Label>(canvas, "waveCounter");
        UpdateWaveCounter(1);

        var stageLabel = UITK.AddElement<Label>(canvas, "stageLabel");
        stageLabel.text = stageName;
        stageLabel.pickingMode = PickingMode.Ignore;

        yield return new WaitForSeconds(2);
        stageLabel.style.opacity = 0;

        yield return null;
    }

    private void InitPauseScreen()
    {
        pauseScreen = UITK.AddElement(canvas, "pauseScreen", "InGameScreen");

        var pauseFrame = UITK.AddElement(pauseScreen, "pauseFrame", "InGameFrame");

        var pauseLabel = UITK.AddElement<Label>(pauseFrame, "pauseLabel", "InGameScreenLabel");
        pauseLabel.text = "Пауза";

        var continueButton = UITK.AddElement<Button>(pauseFrame, "continueButton", "MainButton");
        continueButton.text = "Продолжить";
        continueButton.clicked += () => UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);

        var settingsButton = UITK.AddElement<Button>(pauseFrame, "settingsButton", "MainButton");
        settingsButton.text = "Настройки";
        settingsButton.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        settingsScreen = UIMenu.InitSettingsMenu(audioMixer, out Button saveSettings);
        pauseScreen.Add(settingsScreen);
        UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        saveSettings.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        var restartButton = UITK.AddElement<Button>(pauseFrame, "restartButton", "MainButton");
        restartButton.text = "Начать заново";
        if (BattleM.Instance.IsInfiniteMode) { restartButton.clicked += () => SceneManager.LoadScene(6); }
        else { restartButton.clicked += () => SceneManager.LoadScene(1); }

        var quitButton = UITK.AddElement<Button>(pauseFrame, "quitButton", "MainButton");
        quitButton.text = "Выйти";
        quitButton.clicked += () => SceneManager.LoadScene(0);

        UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
    }

    private void InitVictoryScreen()
    {
        victoryScreen = UITK.AddElement(canvas, "victoryScreen", "InGameScreen");

        var victoryFrame = UITK.AddElement(victoryScreen, "victoryFrame", "InGameFrame");

        var victoryLabel = UITK.AddElement<Label>(victoryFrame, "victoryLabel", "InGameScreenLabel");
        victoryLabel.text = "Победа";

        var scoreLabel = UITK.AddElement<Label>(victoryFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        scoreLabel.text = scoreLabel.text = "Очки: " + GameManager.Instance.totalScore.ToString();
        Enemy.OnScoreGain += (int amount) => scoreLabel.text = "Очки: " + GameManager.Instance.totalScore.ToString();

        if (isFinal)
        {
            var endButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            endButton.text = "Конец";
            endButton.clicked += () => SceneManager.LoadScene(0);
        }
        else
        {
            var nextButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            nextButton.text = "Дальше";
            nextButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        UIMenu.ToggleScreen(victoryScreen, ref isVictoryVisible);
    }

    private void InitDefeatScreen()
    {
        //Defeat Screen
        defeatScreen = UITK.AddElement(canvas, "defeatScreen", "InGameScreen");

        var defeatFrame = UITK.AddElement(defeatScreen, "defeatFrame", "InGameFrame");

        var defeatLabel = UITK.AddElement<Label>(defeatFrame, "defeatLabel", "InGameScreenLabel");
        defeatLabel.text = "Поражение";

        var scoreLabel = UITK.AddElement<Label>(defeatFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        scoreLabel.text = scoreLabel.text = "Очки: " + GameManager.Instance.totalScore.ToString();
        Enemy.OnScoreGain += (int amount) => scoreLabel.text = "Очки: " + GameManager.Instance.totalScore.ToString();

        var restartButton = UITK.AddElement<Button>(defeatFrame, "restartButton", "MainButton");
        restartButton.text = "Начать заново";
        restartButton.clicked += () => SceneManager.LoadScene(1);

        UIMenu.ToggleScreen(defeatScreen, ref isDefeatVisible);
    }

    private void UpdateWaveCounter(int wave)
    {
        waveCounter.text = "Волна " + wave + "/" + BattleM.Instance.TotalWaves;
    }
}
