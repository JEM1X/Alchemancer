using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private IngredientList_SO ingredientList;
    [SerializeField] private Ingredient_SO[] ingredients;
    [SerializeField] private Potion_SO[] potions = new Potion_SO[3];

    private AlchemancerMediator mediator;
    private int drawAmount = 6;

    public event Action<Ingredient_SO[]> OnHandUpdate;
    public event Action<Potion_SO[]> OnPotionUpdate;


    private void Awake()
    {
        mediator = GetComponent<AlchemancerMediator>();
    }

    public void DrawNewHand()
    {
        ingredients = new Ingredient_SO[drawAmount];

        for (int i = 0; i < ingredients.Length; i++) 
        {
            ingredients[i] = ingredientList.Ingredients[UnityEngine.Random.Range(0, ingredientList.Ingredients.Length)];
        }

        OnHandUpdate(ingredients);
    }

    public void CraftNewPotion(Ingredient_SO[] ingredients)
    {
        if (!mediator.Cauldron.TryCombineIngredients(ingredients, out Potion_SO potion)) return;
        
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] != null) continue;

            potions[i] = potion;

            OnPotionUpdate?.Invoke(potions);

            break;
        }
    }
}
