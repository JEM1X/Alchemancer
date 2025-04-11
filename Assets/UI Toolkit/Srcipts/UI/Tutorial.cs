using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;


    private void Start()
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

        var welcomeScreen = UITK.AddElement(canvas, "welcomeScreen", "InGameFrame");

        var welcomeLabel = UITK.AddElement<Label>(welcomeScreen, "welcomeLabel", "MainText");
        welcomeLabel.text = "Добро пожаловать в Alchemancer." +
            "Вы — кобольд-алхимик, которому предстоит пробиться сквозь толпы монстров." +
            "Для победы вам нужно пройти все этапы, каждый из которых состоит из трёх волн врагов." +
            "С каждым новым этапом сложность будет увеличиваться, и вам придётся использовать" +
            "всю свою алхимическую сноровку, чтобы выжить.";

        var welcomeButton = UITK.AddElement<Button>(welcomeScreen, "welcomeButton", "MainButton");
        welcomeButton.text = "OK";
        welcomeButton.clicked += () => welcomeScreen.style.display = DisplayStyle.None;

        var handScreen = UITK.AddElement(canvas, "handScreen", "InGameFrame");
        handScreen.style.display = DisplayStyle.None;
        welcomeButton.clicked += () => handScreen.style.display = DisplayStyle.Flex;

        var handLabel = UITK.AddElement<Label>(handScreen, "handLabel", "ClearText");
        handLabel.text = "В бою вам помогут зелья, которые делятся на три вида: Эликсиры, Капсулы и Колбы." +
            "Эликсиры применяются на игрока, Капсулы воздействуют на одного врага, а Колбы поражают всех врагов сразу." +
            "Однако в начале каждого этапа у вас не будет ни одного зелья." +
            "Чтобы их создать, вам нужно правильно смешивать ингредиенты." +
            "Каждое зелье имеет уникальный рецепт, состоящий из трёх разных ингредиентов." +
            "В начале вашего хода вам будет выдано шесть карт ингредиентов." +
            "Попробуй смешать три карты ингредиентов";

        var handButton = UITK.AddElement<Button>(handScreen, "handButton", "MainButton");
        handButton.text = "OK";
        handButton.clicked += () => handScreen.style.display = DisplayStyle.None;


    }
}
