using UnityEngine;
using UnityEngine.UIElements;

public class UICard
{
    public Button cardFrame;

    protected void InitializeCard(Card_SO card)
    {
        cardFrame = UITK.CreateElement<Button>("cardFrame");

        var cardLabel = UITK.AddElement<Label>(cardFrame, "cardLabel", "MainText");
        cardLabel.text = card.Label;

        var cardIcon = UITK.AddElement(cardFrame, "cardIcon");
        cardIcon.style.backgroundImage = new StyleBackground(card.Icon);

        var cardDescription = UITK.AddElement<Label>(cardFrame, "cardDescription", "SubText");
        cardDescription.text = card.Description;
    }
    
}
