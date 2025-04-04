using UnityEngine;
using UnityEngine.UIElements;

public class PotionCard : UICard
{
    public Potion_SO potion;
    public bool isSelected = false;

    public PotionCard(Potion_SO potion)
    {
        this.potion = potion;
        InitializeCard(potion);
    }

    public bool Select()
    {
        if (isSelected)
        {
            isSelected = false;
            cardFrame.style.scale = StyleKeyword.Null;
            cardFrame.style.translate = StyleKeyword.Null;
            cardFrame.style.rotate = StyleKeyword.Null;

            return false;
        }
        else
        {
            isSelected = true;
            cardFrame.style.scale = new StyleScale(new Vector2(1.2f, 1.2f));
            cardFrame.style.translate = new StyleTranslate(new Translate(60, 0));
            cardFrame.style.rotate = new StyleRotate(new Rotate(new Angle(20)));
            return true;
        }
    }
}