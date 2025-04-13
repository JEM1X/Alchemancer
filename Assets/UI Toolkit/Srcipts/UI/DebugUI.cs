using UnityEngine;
using UnityEngine.UIElements;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private Alchemancer alchemancer;
    [SerializeField] private PotionList_SO potionList;

    private bool isHidden = false;
    private VisualElement canvas;


    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            HideDebug();
        }
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        canvas = UITK.AddElement<ScrollView>(root, "canvas");
        canvas.style.height = 900;
        canvas.style.width = 800;
        canvas.style.alignItems = Align.FlexStart;
        canvas.style.paddingTop = 10;
        canvas.style.paddingLeft = 10;
        HideDebug();

        foreach(Potion_SO potion in potionList.AllPotions)
        {
            var potionButton = UITK.AddElement<Button>(canvas);
            potionButton.text = "Добавить " + potion.Label;
            potionButton.clicked += () => AddPotion(potion);
        }
        
    }

    private void HideDebug()
    {
        if (isHidden)
        {
            canvas.style.display = DisplayStyle.Flex;
            isHidden = false;
        }
        else
        {
            canvas.style.display = DisplayStyle.None;
            isHidden = true;
        }
    }

    private void AddPotion(Potion_SO potion)
    {
        alchemancer.PlayerHand.BrewNewPotion(potion.Ingredients);
    }
}
