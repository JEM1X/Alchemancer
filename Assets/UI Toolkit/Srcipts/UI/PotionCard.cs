using UnityEngine;

public class PotionCard : UICard
{
    public Potion_SO potion;
    public bool isInBelt = true;

    public PotionCard(Potion_SO potion)
    {
        this.potion = potion;
        InitializeCard(potion);
    }
}