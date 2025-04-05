using UnityEngine;
using UnityEngine.UIElements;

public class InGameGuide : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private Sprite sprite;

    private VisualElement background;
    private bool isVisible = true;


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

        background = UITK.AddElement(canvas, "background");

        var guideFrame = UITK.AddElement(background, "guideFrame");

        var guidePage = UITK.AddElement(guideFrame, "guidePage");

        var pageLabel = UITK.AddElement<Label>(guidePage, "pageLabel");
        pageLabel.text = "Вы — кобольд - алхимик, которому предстоит пробиться сквозь толпы монстров." +
            "Для победы вам нужно пройти все этапы, каждый из которых состоит из трёх волн врагов." +
            "С каждым новым этапом сложность будет увеличиваться," +
            "и вам придётся использовать всю свою алхимическую сноровку, чтобы выжить." +
            "В бою вам помогут зелья, которые делятся на три вида: Эликсиры, Капсулы и Колбы." +
            "Эликсиры применяются на игрока, Капсулы воздействуют на одного врага, а Колбы поражают всех врагов сразу." +
            "Однако в начале каждого этапа у вас не будет ни одного зелья.";

        var guidePage1 = UITK.AddElement(guideFrame, "guidePage");

        var page1Label = UITK.AddElement<Label>(guidePage1, "pageLabel");
        page1Label.text = "Чтобы их создать, вам нужно правильно смешивать ингредиенты." +
            "Каждое зелье имеет уникальный рецепт, состоящий из трёх разных ингредиентов." +
            "В начале вашего хода вам будет выдано шесть карт ингредиентов." +
            "Используйте их, чтобы создавать зелья и применять их в бою." +
            "Когда у вас закончатся ингредиенты и зелья, нажмите 'Завершить ход' — тогда ход перейдёт к вашим противникам." +
            "После того как все враги завершат свои ходы, начнётся новый раунд, и ход снова перейдёт к вам." +
            "Ваша цель — победить всех врагов и не погибнуть на пути. Удачи!";

        var toggleButton = UITK.AddElement<Button>(canvas, "toggleButton");
        toggleButton.text = "Гайд";
        toggleButton.style.backgroundImage = new StyleBackground(sprite);
        toggleButton.clicked += () => ToggleGuide();

        ToggleGuide(false);
    }

    private void ToggleGuide(bool sound = true)
    {
        if (isVisible)
        {
            background.style.display = DisplayStyle.None;
            isVisible = false;

            if(sound)
                AudioManager.Instance.PlaySound(AudioManager.Instance.guideSounds[1]);
        }
        else
        {
            background.style.display = DisplayStyle.Flex;
            isVisible = true;
            if(sound)
                AudioManager.Instance.PlaySound(AudioManager.Instance.guideSounds[0]);
        }
    }
}
