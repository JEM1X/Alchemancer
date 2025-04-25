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
        UITK.LocalizeStringUITK(stageLabel, UITK.UITABLE, stageName);
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
        UITK.LocalizeStringUITK(pauseLabel, UITK.UITABLE, "Menu.Pause");

        var continueButton = UITK.AddElement<Button>(pauseFrame, "continueButton", "MainButton");
        continueButton.clicked += () => UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
        UITK.LocalizeStringUITK(continueButton, UITK.UITABLE, "Menu.Continue");

        var settingsButton = UITK.AddElement<Button>(pauseFrame, "settingsButton", "MainButton");
        settingsButton.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);
        UITK.LocalizeStringUITK(settingsButton, UITK.UITABLE, "Menu.Settings");

        settingsScreen = UIMenu.InitSettingsMenu(audioMixer, out Button saveSettings);
        pauseScreen.Add(settingsScreen);

        saveSettings.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        var restartButton = UITK.AddElement<Button>(pauseFrame, "restartButton", "MainButton");
        if (BattleM.Instance.IsInfiniteMode)
        { restartButton.clicked += () => SceneManager.LoadScene(6); }
        else 
        { restartButton.clicked += () => SceneManager.LoadScene(1); }
        UITK.LocalizeStringUITK(restartButton, UITK.UITABLE, "Menu.Restart");

        var quitButton = UITK.AddElement<Button>(pauseFrame, "quitButton", "MainButton");
        quitButton.clicked += () => SceneManager.LoadScene(0);
        UITK.LocalizeStringUITK(quitButton, UITK.UITABLE, "Menu.Quit");

        UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
    }

    private void InitVictoryScreen()
    {
        victoryScreen = UITK.AddElement(canvas, "victoryScreen", "InGameScreen");

        var victoryFrame = UITK.AddElement(victoryScreen, "victoryFrame", "InGameFrame");

        var victoryLabel = UITK.AddElement<Label>(victoryFrame, "victoryLabel", "InGameScreenLabel");
        UITK.LocalizeStringUITK(victoryLabel, UITK.UITABLE, "Menu.Victory");

        var scoreLabel = UITK.AddElement<Label>(victoryFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        UITK.LocalizeStringUITK(scoreLabel, UITK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());
        Enemy.OnScoreGain += (int amount) => 
        UITK.LocalizeStringUITK(scoreLabel, UITK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());

        if (isFinal)
        {
            var endButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            endButton.clicked += () => SceneManager.LoadScene(0);
            UITK.LocalizeStringUITK(endButton, UITK.UITABLE, "Menu.End");
        }
        else
        {
            var nextButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            nextButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            UITK.LocalizeStringUITK(nextButton, UITK.UITABLE, "Menu.Next");
        }

        UIMenu.ToggleScreen(victoryScreen, ref isVictoryVisible);
    }

    private void InitDefeatScreen()
    {
        defeatScreen = UITK.AddElement(canvas, "defeatScreen", "InGameScreen");

        var defeatFrame = UITK.AddElement(defeatScreen, "defeatFrame", "InGameFrame");

        var defeatLabel = UITK.AddElement<Label>(defeatFrame, "defeatLabel", "InGameScreenLabel");
        UITK.LocalizeStringUITK(defeatLabel, UITK.UITABLE, "Menu.Defeat");

        var scoreLabel = UITK.AddElement<Label>(defeatFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        UITK.LocalizeStringUITK(scoreLabel, UITK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());
        Enemy.OnScoreGain += (int amount) =>
        UITK.LocalizeStringUITK(scoreLabel, UITK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());

        var restartButton = UITK.AddElement<Button>(defeatFrame, "restartButton", "MainButton");
        restartButton.clicked += () => SceneManager.LoadScene(1);
        UITK.LocalizeStringUITK(restartButton, UITK.UITABLE, "Menu.Restart");

        UIMenu.ToggleScreen(defeatScreen, ref isDefeatVisible);
    }

    private void UpdateWaveCounter(int wave)
    {
        UITK.LocalizeStringUITK(waveCounter, UITK.UITABLE, "Menu.Wave", wave + "/" + BattleM.Instance.TotalWaves);
    }

    public void TogglePauseScreen()
    {
        UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
    }
}
