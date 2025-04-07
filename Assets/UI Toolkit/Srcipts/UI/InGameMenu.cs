using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private AlchemancerMediator mediator;
    [SerializeField] private string stageName;


    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement canvas;
    private VisualElement pauseScreen;
    private bool isPauseVisible = true;
    private VisualElement victoryScreen;
    private bool isVictoryVisible = true;
    private VisualElement defeatScreen;
    private bool isDefeatVisible = true;
    private Label waveCounter;

    private void Awake()
    {
        InitializeUI();
    }

    private void Start()
    {
        BattleManager.Instance.OnAllWavesCleared += ToggleVictoryScreen;
        BattleManager.Instance.OnPlayerLose += ToggleDefeatScreen;
        BattleManager.Instance.OnWaveStart += UpdateWaveCounter;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseScreen();
        }
    }

    private void InitializeUI()
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

        var stageLabel = UITK.AddElement<Label>(canvas, "stageLabel");
        stageLabel.text = stageName;
        stageLabel.pickingMode = PickingMode.Ignore;
        BattleManager.Instance.OnWaveStart += (int amount) => stageLabel.style.opacity = 0;
    }

    private void InitPauseScreen()
    {
        pauseScreen = UITK.AddElement(canvas, "pauseScreen", "InGameScreen");

        var pauseFrame = UITK.AddElement(pauseScreen, "pauseFrame", "InGameFrame");

        var pauseLabel = UITK.AddElement<Label>(pauseFrame, "pauseLabel", "InGameScreenLabel");
        pauseLabel.text = "�����";

        var continueButton = UITK.AddElement<Button>(pauseFrame, "continueButton", "MainButton");
        continueButton.text = "����������";
        continueButton.clicked += TogglePauseScreen;

        var restartButton = UITK.AddElement<Button>(pauseFrame, "restartButton", "MainButton");
        restartButton.text = "������ ������";
        restartButton.clicked += () => SceneManager.LoadScene(1);

        var quitButton = UITK.AddElement<Button>(pauseFrame, "quitButton", "MainButton");
        quitButton.text = "�����";
        quitButton.clicked += () => SceneManager.LoadScene(0);

        TogglePauseScreen();
    }

    private void InitVictoryScreen()
    {
        victoryScreen = UITK.AddElement(canvas, "victoryScreen", "InGameScreen");

        var victoryFrame = UITK.AddElement(victoryScreen, "victoryFrame", "InGameFrame");

        var victoryLabel = UITK.AddElement<Label>(victoryFrame, "victoryLabel", "InGameScreenLabel");
        victoryLabel.text = "������";

        var nextButton = UITK.AddElement<Button>(victoryFrame, "nextButton", "MainButton");
        nextButton.text = "������";
        nextButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        ToggleVictoryScreen();
    }

    private void InitDefeatScreen()
    {
        //Defeat Screen
        defeatScreen = UITK.AddElement(canvas, "defeatScreen", "InGameScreen");

        var defeatFrame = UITK.AddElement(defeatScreen, "defeatFrame", "InGameFrame");

        var defeatLabel = UITK.AddElement<Label>(defeatFrame, "defeatLabel", "InGameScreenLabel");
        defeatLabel.text = "���������";

        var restartButton = UITK.AddElement<Button>(defeatFrame, "restartButton", "MainButton");
        restartButton.text = "������ ������";
        restartButton.clicked += () => SceneManager.LoadScene(1);

        ToggleDefeatScreen();
    }

    private void TogglePauseScreen()
    {
        if (isPauseVisible)
        {
            pauseScreen.style.display = DisplayStyle.None;
            isPauseVisible = false;
        }
        else
        {
            pauseScreen.style.display = DisplayStyle.Flex;
            isPauseVisible = true;
        }
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

    private void UpdateWaveCounter(int wave)
    {
        waveCounter.text = "����� " + wave + "/" + BattleManager.Instance.TotalWaves;
    }
}
