using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CombatUI : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Alchemancer alchemancer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLibraire audioLibraire;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private UICombatStyle_SO combatStyle;

    [Header ("Animations")]
    [SerializeField] private bool isCardAnim = false;
    [SerializeField] private float drawDuration = 0.4f;
    [SerializeField] private float overlapDuration = 0.5f;

    private Button hordeUI;
    private VisualElement handUI;
    private VisualElement bagUI;
    private VisualElement beltUI;
    private VisualElement interactionUI;

    private List<IngredientCard> cardsInHand = new(0);
    private List<IngredientCard> cardsInCauldron = new(0);
    private PotionCard potentialPotion = null;
    private PotionCard potionInUse = null;

    private void Awake()
    {
        InitializeUI();

        PlayerHand.OnNewIngredient += InitializeIngredientCard;
        PlayerHand.OnIngredientUse += RemoveIngredientCard;
        PlayerHand.OnNewPotion += InitializePotionCard;
        alchemancer.PlayerCombat.OnSpawn += (Combatant combatant) => InitializePlayer((PlayerCombat)combatant);
        alchemancer.PlayerCombat.OnTurnStart += ShowHand;
        BattleM.Instance.Horde.OnNewEnemy += InitializeEnemy;
    }

    private void InitializeUI()
    {

        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");
        canvas.style.height = new Length(100, LengthUnit.Percent);
        canvas.pickingMode = PickingMode.Ignore;

        hordeUI = UITK.AddElement<Button>(canvas, "ClearButton", "hordeUI");
        hordeUI.clicked += TargetEnemies;

        handUI = UITK.AddElement(canvas, "handUI");

        interactionUI = UITK.AddElement(handUI, "interactionUI");

        bagUI = UITK.AddElement(handUI, "bagUI");

        beltUI = UITK.AddElement(handUI, "beltUI");

        var endTurnButton = UITK.AddElement<Button>(interactionUI, "endTurnButton", "MainButton");
        endTurnButton.style.backgroundImage = new StyleBackground(combatStyle.forward);
        endTurnButton.clicked += () =>
        {
            ClearHand();
            bagUI.Clear();
            HideHand();
            alchemancer.PlayerCombat.CompletePlayerTurn?.Invoke();

            audioSource.PlayOneShot(audioLibraire.uiSounds[0]);
        };

        var brewButton = UITK.AddElement<Button>(interactionUI, "brewButton", "MainButton");
        brewButton.style.backgroundImage = new StyleBackground(combatStyle.cauldron);
        brewButton.clicked += BrewPotion;
    }

    private void InitializeIngredientCard(Ingredient_SO ingredient)
    {
        var ingredientCard = new IngredientCard(ingredient);
        ingredientCard.cardFrame.clicked += () => SelectIngredientCard(ingredientCard);

        cardsInHand.Add(ingredientCard);
        bagUI.Add(ingredientCard.cardFrame);

        StartCoroutine(DrawCardAnim(ingredientCard, new Vector2Int(1700, 100)));

        ingredientCard.cardFrame.RegisterCallback<PointerEnterEvent>(evt =>
            audioSource.PlayOneShot(audioLibraire.cardSounds[0]));
    }

    private void RemoveIngredientCard(Ingredient_SO ingredient)
    {
        IngredientCard ingredientCard = cardsInCauldron.FirstOrDefault(card => card.ingredient == ingredient);

        if (ingredientCard != null)
        {
            cardsInHand.Remove(ingredientCard);
            cardsInCauldron.Remove(ingredientCard);
            ingredientCard.cardFrame.RemoveFromHierarchy();
            return;
        }

        ingredientCard = cardsInHand.FirstOrDefault(card => card.ingredient == ingredient);

        if (ingredientCard != null)
        {
            cardsInHand.Remove(ingredientCard);
            ingredientCard.cardFrame.RemoveFromHierarchy();
            return;
        }

        Debug.LogWarning("Could not find IngredientCard for Ingredient");
    }

    private void InitializePotionCard(Potion_SO potion)
    {
        if (potion == null) return;

        var potionCard = new PotionCard(potion);
        potionCard.cardFrame.clicked += () => SelectPotionCard(potionCard);
       
        beltUI.Add(potionCard.cardFrame);
        StartCoroutine(DrawCardAnim(potionCard, new Vector2Int(0, 500)));

        potionCard.cardFrame.RegisterCallback<PointerEnterEvent>(evt =>
            audioSource.PlayOneShot(audioLibraire.cardSounds[0]));
    }

    private void InitializeEnemy(Enemy enemy)
    {
        var enemyCard = new EnemyCard(enemy, combatStyle, mainCamera);
        hordeUI.Add(enemyCard.combatantFrame);
        enemyCard.combatantFrame.clicked += () => TargetEnemy(enemy);
    }

    private void InitializePlayer(PlayerCombat player)
    {
        var playerCard = new CombatantCard(player, combatStyle, mainCamera);
        hordeUI.Add(playerCard.combatantFrame);
        playerCard.combatantFrame.clicked += TargetPlayer;
    }

    private void SelectIngredientCard(IngredientCard card)
    {
        if (!card.isSelected && cardsInCauldron.Count >= 3) return;

        if (card.Select())
        {
            cardsInCauldron.Add(card);
            audioSource.PlayOneShot(audioLibraire.cardSounds[1]);
        }
        else
        {
            cardsInCauldron.Remove(card);
            audioSource.PlayOneShot(audioLibraire.cardSounds[2]);
        }

        UpdatePotentialPotion();
    }

    private void UpdatePotentialPotion()
    {
        Potion_SO potion = CheckForPotentialPotion();

        if (potion == null)
        {
            HidePotentialPotion();
            return;
        }
        else
        {
            ShowPotentialPotion(potion);
        }
    }

    private Potion_SO CheckForPotentialPotion()
    {
        if (cardsInCauldron.Count <= 1) return null;

        Ingredient_SO[] ingredients = cardsInCauldron.Select(card => card.ingredient).ToArray();

        if (ingredients.Distinct().Count() <= 1) return null;

        return GameManager.Instance.discoveredPotions.FirstOrDefault(potion => potion.IsinRecipe(ingredients));
    }

    private void ShowPotentialPotion(Potion_SO potion)
    {
        HidePotentialPotion();

        potentialPotion = new(potion);
        handUI.Add(potentialPotion.cardFrame);
        potentialPotion.cardFrame.style.position = Position.Absolute;
        potentialPotion.cardFrame.style.alignSelf = Align.Center;
        potentialPotion.cardFrame.style.top = 300;
        potentialPotion.cardFrame.style.opacity = 0.8f;
    }

    private void HidePotentialPotion()
    {
        potentialPotion?.cardFrame.RemoveFromHierarchy();
        potentialPotion = null;
    }

    private void SelectPotionCard(PotionCard card)
    {
        if (!card.isSelected && potionInUse != null)
            SelectPotionCard(potionInUse);

        if (card.Select())
        {
            potionInUse = card;
            audioSource.PlayOneShot(audioLibraire.cardSounds[1]);
        }
        else
        {
            potionInUse = null;
            audioSource.PlayOneShot(audioLibraire.cardSounds[2]);
        }
    }

    private void BrewPotion()
    {
        if (cardsInCauldron.Count < 2) return;
        if (beltUI.childCount >= 3) return;

        Ingredient_SO[] usedIngredients = cardsInCauldron.Select(card => card.ingredient).ToArray();

        alchemancer.PlayerHand.BrewNewPotion(usedIngredients);

        UpdatePotentialPotion();

        audioSource.PlayOneShot(audioLibraire.potionSounds[0]);
    }

    private void ClearHand()
    {
        cardsInHand = new(0);
        cardsInCauldron = new(0);
    }

    private void TargetEnemy(Enemy enemy)
    {
        if (potionInUse?.potion is Capsule_SO capsule)
        {
            alchemancer.PlayerHand.UseCapsule(capsule, enemy);
            potionInUse.cardFrame.RemoveFromHierarchy();
            potionInUse = null;

            audioSource.PlayOneShot(audioLibraire.potionSounds[3]);
        }
    }

    private void TargetPlayer()
    {
        if (potionInUse?.potion is Elixir_SO elixir)
        {
            alchemancer.PlayerHand.UseElixir(elixir);
            potionInUse.cardFrame.RemoveFromHierarchy();
            potionInUse = null;

            audioSource.PlayOneShot(audioLibraire.potionSounds[1]);
        }
    }

    private void TargetEnemies()
    {
        if (potionInUse?.potion is Flask_SO flask)
        {
            alchemancer.PlayerHand.UseFlask(flask);
            potionInUse.cardFrame.RemoveFromHierarchy();
            potionInUse = null;

            audioSource.PlayOneShot(audioLibraire.potionSounds[2]);
        }
    }

    private void ShowHand()
    {
        handUI.SetEnabled(true);
        handUI.pickingMode = PickingMode.Position;
    }

    private void HideHand()
    {
        handUI.SetEnabled(false);
        handUI.pickingMode = PickingMode.Ignore;
    }

    private IEnumerator DrawCardAnim(UICard card, Vector2Int pos)
    {
        card.cardFrame.style.translate = new StyleTranslate(new Translate(new Length(pos.x), new Length(pos.y)));

        while (isCardAnim)
        {
            yield return null;
        }

        isCardAnim = true;

        bool nextCard = true;
        float duration = 0;
        while (duration < 1f)
        {
            if (duration > overlapDuration && nextCard)
            {
                isCardAnim = false;
                nextCard = false;
            }

            Length xPos = new Length(pos.x * (1 - Easing.InOutCubic(duration)));
            Length yPos = new Length(pos.y * (1 - Easing.InOutCubic(duration)));
            card.cardFrame.style.translate = new StyleTranslate( new Translate(xPos, yPos));
            duration += Time.deltaTime / drawDuration;
            yield return null;
        }

        audioSource.PlayOneShot(audioLibraire.cardSounds[1]);
        card.cardFrame.style.translate = new StyleTranslate(StyleKeyword.Null);
    }

    private void OnDestroy()
    {
        PlayerHand.OnNewIngredient -= InitializeIngredientCard;
        PlayerHand.OnIngredientUse -= RemoveIngredientCard;
        PlayerHand.OnNewPotion -= InitializePotionCard;
    }
}
