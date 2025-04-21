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
        LTK.LocalizeStringUITK(stageLabel, LTK.UITABLE, stageName);
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
        LTK.LocalizeStringUITK(pauseLabel, LTK.UITABLE, "Menu.Pause");

        var continueButton = UITK.AddElement<Button>(pauseFrame, "continueButton", "MainButton");
        continueButton.clicked += () => UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
        LTK.LocalizeStringUITK(continueButton, LTK.UITABLE, "Menu.Continue");

        var settingsButton = UITK.AddElement<Button>(pauseFrame, "settingsButton", "MainButton");
        settingsButton.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);
        LTK.LocalizeStringUITK(settingsButton, LTK.UITABLE, "Menu.Settings");

        settingsScreen = UIMenu.InitSettingsMenu(audioMixer, out Button saveSettings);
        pauseScreen.Add(settingsScreen);

        saveSettings.clicked += () => UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        UIMenu.ToggleScreen(settingsScreen, ref isSettingsVisible);

        var restartButton = UITK.AddElement<Button>(pauseFrame, "restartButton", "MainButton");
        if (BattleM.Instance.IsInfiniteMode)
        { restartButton.clicked += () => SceneManager.LoadScene(6); }
        else 
        { restartButton.clicked += () => SceneManager.LoadScene(1); }
        LTK.LocalizeStringUITK(restartButton, LTK.UITABLE, "Menu.Restart");

        var quitButton = UITK.AddElement<Button>(pauseFrame, "quitButton", "MainButton");
        quitButton.clicked += () => SceneManager.LoadScene(0);
        LTK.LocalizeStringUITK(quitButton, LTK.UITABLE, "Menu.Quit");

        UIMenu.ToggleScreen(pauseScreen, ref isPauseVisible);
    }

    private void InitVictoryScreen()
    {
        victoryScreen = UITK.AddElement(canvas, "victoryScreen", "InGameScreen");

        var victoryFrame = UITK.AddElement(victoryScreen, "victoryFrame", "InGameFrame");

        var victoryLabel = UITK.AddElement<Label>(victoryFrame, "victoryLabel", "InGameScreenLabel");
        LTK.LocalizeStringUITK(victoryLabel, LTK.UITABLE, "Menu.Victory");

        var scoreLabel = UITK.AddElement<Label>(victoryFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        LTK.LocalizeStringUITK(scoreLabel, LTK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());
        Enemy.OnScoreGain += (int amount) => 
        LTK.LocalizeStringUITK(scoreLabel, LTK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());

        if (isFinal)
        {
            var endButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            endButton.clicked += () => SceneManager.LoadScene(0);
            LTK.LocalizeStringUITK(endButton, LTK.UITABLE, "Menu.End");
        }
        else
        {
            var nextButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
            nextButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            LTK.LocalizeStringUITK(nextButton, LTK.UITABLE, "Menu.Next");
        }

        UIMenu.ToggleScreen(victoryScreen, ref isVictoryVisible);
    }

    private void InitDefeatScreen()
    {
        defeatScreen = UITK.AddElement(canvas, "defeatScreen", "InGameScreen");

        var defeatFrame = UITK.AddElement(defeatScreen, "defeatFrame", "InGameFrame");

        var defeatLabel = UITK.AddElement<Label>(defeatFrame, "defeatLabel", "InGameScreenLabel");
        LTK.LocalizeStringUITK(defeatLabel, LTK.UITABLE, "Menu.Defeat");

        var scoreLabel = UITK.AddElement<Label>(defeatFrame, "scoreLabel", "MainText", "InGameScreenLabel");
        LTK.LocalizeStringUITK(scoreLabel, LTK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());
        Enemy.OnScoreGain += (int amount) =>
        LTK.LocalizeStringUITK(scoreLabel, LTK.UITABLE, "Menu.Score", GameManager.Instance.totalScore.ToString());

        var restartButton = UITK.AddElement<Button>(defeatFrame, "restartButton", "MainButton");
        restartButton.clicked += () => SceneManager.LoadScene(1);
        LTK.LocalizeStringUITK(restartButton, LTK.UITABLE, "Menu.Restart");

        UIMenu.ToggleScreen(defeatScreen, ref isDefeatVisible);
    }

    private void UpdateWaveCounter(int wave)
    {
        LTK.LocalizeStringUITK(waveCounter, LTK.UITABLE, "Menu.Wave", wave + "/" + BattleM.Instance.TotalWaves);
    }
}
