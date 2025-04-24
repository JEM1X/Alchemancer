using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private Alchemancer alchemancer;

    private VisualElement canvas;
    private bool isPotionShown = false;

    private void Start()
    {
        InitializeUI();
        PlayerHand.OnNewPotion += (Potion_SO potion) => InitPotion();
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        canvas = UITK.AddElement(root, "canvas", "MainText");

        var welcomeScreen = UITK.AddElement(canvas, "welcomeScreen", "InGameFrame");

        var welcomeLabel = UITK.AddElement<Label>(welcomeScreen, "welcomeLabel", "ClearText");
        UITK.LocalizeStringUITK(welcomeLabel, UITK.UITABLE, "Tutorial.Intro");

        var welcomeButton = UITK.AddElement<Button>(welcomeScreen, "welcomeButton", "MainButton");
        welcomeButton.text = "OK";
        welcomeButton.clicked += () => welcomeScreen.style.display = DisplayStyle.None;

        var handScreen = UITK.AddElement(canvas, "handScreen", "InGameFrame");
        handScreen.style.display = DisplayStyle.None;
        welcomeButton.clicked += () => handScreen.style.display = DisplayStyle.Flex;

        var handLabel = UITK.AddElement<Label>(handScreen, "handLabel", "ClearText");
        UITK.LocalizeStringUITK(handLabel, UITK.UITABLE, "Tutorial.Hand");

        var handButton = UITK.AddElement<Button>(handScreen, "handButton", "MainButton");
        handButton.text = "OK";
        handButton.clicked += () => handScreen.style.display = DisplayStyle.None;
    }

    private void InitPotion()
    {
        if (isPotionShown) return;

        Debug.Log("aboba");

        var potionScreen = UITK.AddElement(canvas, "potionScreen", "InGameFrame");
        potionScreen.style.display = DisplayStyle.Flex;

        var potionLabel = UITK.AddElement<Label>(potionScreen, "potionLabel", "ClearText");
        UITK.LocalizeStringUITK(potionLabel, UITK.UITABLE, "Tutorial.Potion");

        var potionButton = UITK.AddElement<Button>(potionScreen, "potionButton", "MainButton");
        potionButton.text = "OK";
        potionButton.clicked += () => potionScreen.style.display = DisplayStyle.None;

        BattleM.Instance.Alchemancer.PlayerCombat.OnTurnStart += InitFinale;

        isPotionShown = true;
    }

    private void InitFinale()
    {
        var finaleScreen = UITK.AddElement(canvas, "finaleScreen", "InGameFrame");
        finaleScreen.style.display = DisplayStyle.Flex;

        var finaleLabel = UITK.AddElement<Label>(finaleScreen, "finaleLabel", "ClearText");
        UITK.LocalizeStringUITK(finaleLabel, UITK.UITABLE, "Tutorial.Finale");

        var finaleButton = UITK.AddElement<Button>(finaleScreen, "finaleButton", "MainButton");
        finaleButton.text = "OK";
        finaleButton.clicked += () => SceneManager.LoadScene(0);
    }
}
