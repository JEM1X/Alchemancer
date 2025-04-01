using UnityEngine;

public class Cauldron : MonoBehaviour
{
    private AlchemancerMediator mediator;
    [SerializeField] private RecipeList_SO RecipeList;


    private void Awake()
    {
        mediator = GetComponent<AlchemancerMediator>();
    }

    public bool TryCombineIngredients(Ingredient_SO[] ingredients, out Potion_SO craftedPotion)
    {
        foreach (Potion_SO potion in RecipeList.PotionRecipes)
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (!potion.IsinRecipe(ingredients[i])) break;

                if (i == ingredients.Length - 1)
                {
                    craftedPotion = potion;
                    return true;
                }
            }
        }

        craftedPotion = null;
        return false;
    }
}
