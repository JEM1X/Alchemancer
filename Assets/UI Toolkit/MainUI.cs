using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour
{
    [SerializeField] private AlchemancerMediator mediator;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement handUI;
    private VisualElement cauldronUI;
    private VisualElement beltUI;
    [SerializeField] private List<IngredientCard> cardsInCauldron = new List<IngredientCard>(0);
    //private Dictionary<Ingredient_SO, IngredientCard> ingredientCardDict;

    private void Awake()
    {
        //StartCoroutine(InitializeUI());
        InitializeUI();

        mediator.PlayerHand.OnHandUpdate += InitializeHand;
        mediator.PlayerHand.OnPotionUpdate += InitializeBelt;
    }

    //private void OnValidate()
    //{
    //    if (Application.isPlaying)
    //        return;

    //    StartCoroutine(InitializeUI());
    //}

    private void OnDestroy()
    {
        mediator.PlayerHand.OnHandUpdate -= InitializeHand;
        mediator.PlayerHand.OnPotionUpdate -= InitializeBelt;

    }

    private void InitializeUI()
    {
        //yield return null;

        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        handUI = UITK.AddElement(canvas, "handUI");

        cauldronUI = UITK.AddElement(canvas, "cauldronUI");

        beltUI = UITK.AddElement(canvas, "beltUI");

        var endTurnButton = UITK.AddElement<Button>(canvas, "endTurnButton");
        endTurnButton.text = "Завершить Ход";
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
            ingredientCard.cardFrame.clicked += () => MoveCauldronHand(ingredientCard);
            
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

            beltUI.Add(potionsCard.cardFrame);
        }
    }

    private void MoveCauldronHand(IngredientCard card)
    {
        if (card.isInHand)
        {
            if (cardsInCauldron.Count >= 3) return;

            cauldronUI.Add(card.cardFrame);
            card.isInHand = false;
            cardsInCauldron.Add(card);
        }
        else
        {
            handUI.Add(card.cardFrame);
            card.isInHand = true;
            cardsInCauldron.Remove(card);
        }
    }

    private void BrewPotion()
    {
        List<Ingredient_SO> ingredients = new List<Ingredient_SO>(0);

        foreach(IngredientCard card  in cardsInCauldron)
        {
            ingredients.Add(card.ingredient);
        }

        mediator.PlayerHand.CraftNewPotion(ingredients.ToArray());
        cardsInCauldron = new List<IngredientCard>(0);
        cauldronUI.Clear();
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

        public PotionCard(Potion_SO potion)
        {
            this.potion = potion;
            InitializeCard(potion);
        }
    }
}
