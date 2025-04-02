using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Horde horde;
    [SerializeField] private AlchemancerMediator mediator;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement handUI;
    private VisualElement beltUI;
    private VisualElement hordeUI;

    private List<IngredientCard> cardsInCauldron = new List<IngredientCard>(0);
    private PotionCard potionInUse = null;

    private void Awake()
    {
        InitializeUI();

        mediator.PlayerHand.OnHandChange += InitializeHand;
        mediator.PlayerHand.OnPotionChange += InitializeBelt;
        horde.OnNewEnemy += InitializeEnemy;
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        handUI = UITK.AddElement(canvas, "handUI");

        beltUI = UITK.AddElement(canvas, "beltUI");

        hordeUI = UITK.AddElement(canvas, "hordeUI");

        var endTurnButton = UITK.AddElement<Button>(canvas, "endTurnButton");
        endTurnButton.text = "Завершить Ход";
        endTurnButton.clicked += ClearCauldron;
        endTurnButton.clicked += mediator.PlayerHand.DrawNewHand;

        var brewButton = UITK.AddElement<Button>(canvas, "brewButton");
        brewButton.text = "Сварить";
        brewButton.clicked += BrewPotion;
    }

    private void InitializeHand(Ingredient_SO[] ingredients)
    {
        handUI.Clear();

        foreach (Ingredient_SO ingredient in ingredients)
        {
            var ingredientCard = new IngredientCard(ingredient);
            ingredientCard.cardFrame.clicked += () => UseIngredientCard(ingredientCard);
            
            handUI.Add(ingredientCard.cardFrame);
        }
    }

    private void InitializeBelt(Potion_SO[] potions)
    {
        beltUI.Clear();

        foreach (Potion_SO potion in potions)
        {
            if (potion == null) continue;

            var potionsCard = new PotionCard(potion);
            potionsCard.cardFrame.clicked += () => UsePotionCard(potionsCard);

            beltUI.Add(potionsCard.cardFrame);
        }
    }

    private void InitializeEnemy(Enemy enemy)
    { 
        Vector2 enemyScreenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
        Vector2 framePos = new Vector2(enemyScreenPos.x, Screen.height - enemyScreenPos.y);

        var enemyFrame = UITK.AddElement<Button>(hordeUI, "enemyFrame");
        enemyFrame.clicked += () => AttackEnemy(enemy);
        enemyFrame.style.top = framePos.y - 150 - 120;
        enemyFrame.style.left = framePos.x - 980 - 70;

        var healthFrame = UITK.AddElement(enemyFrame, "healthFrame");

        var healthAmount = UITK.AddElement<Label>(healthFrame, "healthAmount");
        healthAmount.text = enemy.Health.ToString();
    }

    private void UseIngredientCard(IngredientCard card)
    {
        if (card.isInHand)
        {
            if (cardsInCauldron.Count >= 3) return;

            card.isInHand = false;
            cardsInCauldron.Add(card);
            card.cardFrame.style.scale = new StyleScale(new Vector2(1.2f, 1.2f));
            card.cardFrame.style.translate = new StyleTranslate(new Translate(0, -60));
        }
        else
        {
            card.isInHand = true;
            cardsInCauldron.Remove(card);
            card.cardFrame.style.scale = StyleKeyword.Null;
            card.cardFrame.style.translate = StyleKeyword.Null;
        }
    }

    private void BrewPotion()
    {
        if (cardsInCauldron.Count < 3) return;
        if (beltUI.childCount >= 3) return; 

        Ingredient_SO[] usedIngredients = new Ingredient_SO[cardsInCauldron.Count];

        for (int i = 0; i < cardsInCauldron.Count; i++)
            usedIngredients[i] = cardsInCauldron[i].ingredient;

        mediator.PlayerHand.CraftNewPotion(usedIngredients);

        //Removing used card
        foreach (IngredientCard card in cardsInCauldron)
            card.cardFrame.RemoveFromHierarchy();

        ClearCauldron();
    }

    private void ClearCauldron()
    {
        cardsInCauldron = new List<IngredientCard>(0);
    }

    private void UsePotionCard(PotionCard card)
    {
        if (card.isInBelt)
        {
            if (potionInUse != null)
                UsePotionCard(potionInUse);
            
            potionInUse = card;
            card.isInBelt = false;
            card.cardFrame.style.scale = new StyleScale(new Vector2(1.2f, 1.2f));
            card.cardFrame.style.translate = new StyleTranslate(new Translate(60, 0));
        }
        else
        {
            potionInUse = null;
            card.isInBelt = true;
            card.cardFrame.style.scale = StyleKeyword.Null;
            card.cardFrame.style.translate = StyleKeyword.Null;
        }
    }

    private void AttackEnemy(Enemy enemy)
    {
        if (potionInUse?.potion is Capsule_SO capsule)
        {
            mediator.PlayerHand.UseCapsule(capsule, enemy);
            potionInUse.cardFrame.RemoveFromHierarchy();
            potionInUse = null;
        }
    }

    private class Card
    {
        public Button cardFrame;

        protected void InitializeCard(Card_SO card)
        {
            cardFrame = UITK.CreateElement<Button>("cardFrame");

            var cardLabel = UITK.AddElement<Label>(cardFrame, "cardLabel", "MainText");
            cardLabel.text = card.Label;

            var cardIcon = UITK.AddElement(cardFrame, "cardIcon");

            var cardDescription = UITK.AddElement<Label>(cardFrame, "cardDescription", "SubText");
            cardDescription.text = card.Description;
        }
    }

    private class IngredientCard : Card
    {
        public Ingredient_SO ingredient;
        public bool isInHand = true;

        public IngredientCard(Ingredient_SO ingredient)
        {
            this.ingredient = ingredient;
            InitializeCard(ingredient);
        }
    }

    private class PotionCard : Card
    {
        public Potion_SO potion;
        public bool isInBelt = true;

        public PotionCard(Potion_SO potion)
        {
            this.potion = potion;
            InitializeCard(potion);
        }
    }
}
