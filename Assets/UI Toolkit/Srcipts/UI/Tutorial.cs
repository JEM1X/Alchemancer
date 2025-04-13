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
        alchemancer.PlayerHand.OnNewPotion += (Potion_SO potion) => InitPotion();
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
        welcomeLabel.text = "Добро пожаловать в Alchemancer. " +
            "Вы — кобольд-алхимик, которому предстоит пробиться сквозь толпы монстров. " +
            "Для победы вам нужно пройти все этапы, каждый из которых состоит из трёх волн врагов. " +
            "С каждым новым этапом сложность будет увеличиваться, и вам придётся использовать " +
            "всю свою алхимическую сноровку, чтобы выжить.";

        var welcomeButton = UITK.AddElement<Button>(welcomeScreen, "welcomeButton", "MainButton");
        welcomeButton.text = "OK";
        welcomeButton.clicked += () => welcomeScreen.style.display = DisplayStyle.None;

        var handScreen = UITK.AddElement(canvas, "handScreen", "InGameFrame");
        handScreen.style.display = DisplayStyle.None;
        welcomeButton.clicked += () => handScreen.style.display = DisplayStyle.Flex;

        var handLabel = UITK.AddElement<Label>(handScreen, "handLabel", "ClearText");
        handLabel.text = "В бою вам помогут зелья, которые делятся на три вида: Эликсиры, Капсулы и Колбы. " +
            "Эликсиры применяются на игрока, Капсулы воздействуют на одного врага, а Колбы поражают всех врагов сразу. " +
            "Однако в начале каждого этапа у вас не будет ни одного зелья. " +
            "Чтобы их создать, вам нужно правильно смешивать ингредиенты. " +
            "Каждое зелье имеет уникальный рецепт, состоящий из трёх разных ингредиентов. " +
            "В начале вашего хода вам будет выдано шесть карт ингредиентов. " +
            "Попробуй смешать три карты ингредиентов";

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
        potionLabel.text = "Поздравляем с первым зельем! Каждое зелье имеет свой уникальный эффект — " +
            "это может быть обычный урон или воздействие, которое усиливает или ослабляет различные аспекты врагов или тебя самого. " +
            "В твоей руке может находиться не более трёх зелий одновременно — если их уже три, создать новое не получится. " +
            "Когда у тебя закончатся карты ингредиентов, нажми 'Конец хода', чтобы передать инициативу противникам.";

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
        finaleLabel.text = "В начале каждого хода ты получаешь 6 случайных карт ингредиентов. " +
            "Однако в конце хода все неиспользованные ингредиенты исчезают. " +
            "Это не касается зелий — они остаются у тебя между ходами, но будут сброшены при переходе на новый этап. " +
            "Рецепты зелий генерируются случайным образом в начале каждого забега, так что тебе придётся открывать их заново. " +
            "Все созданные тобой зелья автоматически записываются в Книгу рецептов, " +
            "которую ты можешь открыть в левом верхнем углу экрана. " +
            "На этом всё.Удачи, алхимик!";

        var finaleButton = UITK.AddElement<Button>(finaleScreen, "finaleButton", "MainButton");
        finaleButton.text = "OK";
        finaleButton.clicked += () => SceneManager.LoadScene(0);
    }
}
