using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class MainUI : MonoBehaviour
{
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;

    private VisualElement handUI;


    private void Awake()
    {
        //StartCoroutine(InitializeUI());
        InitializeUI();

        playerHand.OnDrawNewHand += InitializeHand;
    }

    //private void OnValidate()
    //{
    //    if (Application.isPlaying)
    //        return;

    //    StartCoroutine(InitializeUI());
    //}

    private void OnDestroy()
    {
        playerHand.OnDrawNewHand -= InitializeHand;
    }

    public void InitializeUI()
    {
        //yield return null;

        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        handUI = UITK.AddElement(canvas, "handUI");
    }

    private void InitializeHand(Ingredient_SO[] ingredients)
    {
        foreach (Ingredient_SO ingredient in ingredients)
        {
            var ingredientCard = new IngredientCard(ingredient);
            handUI.Add(ingredientCard.card);
        }
    }

    private class IngredientCard
    {
        public Ingredient_SO ingredient;
        public VisualElement card;


        public IngredientCard(Ingredient_SO ingredient)
        {
            this.ingredient = ingredient;
            card = InitializeCard();
        }

        private VisualElement InitializeCard()
        {
            var cardFrame = UITK.CreateElement("cardFrame");

            var cardLabel = UITK.AddElement<Label>(cardFrame, "cardLabel", "MainText");
            cardLabel.text = ingredient.Label;

            var cardIcon = UITK.AddElement(cardFrame, "cardIcon");

            var cardDescription = UITK.AddElement<Label>(cardFrame, "cardDescription", "SubText");
            cardDescription.text = ingredient.Description;

            return cardFrame;
        }
    }
}
