using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CombatUI : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AlchemancerMediator mediator;

    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private UIStyle_SO styleSheet;
    [SerializeField] private UICombatStyle_SO combatStyle;

    private VisualElement hordeUI;
    private VisualElement handUI;
    private VisualElement bagUI;
    private VisualElement beltUI;
    private VisualElement interactionUI;

    private List<IngredientCard> cardsInCauldron = new List<IngredientCard>(0);
    private PotionCard potionInUse = null;
    private List<CombatantCard> enemyCards;
    private bool isHandVisible = false;

    private void Awake()
    {
        InitializeUI();

        mediator.PlayerHand.OnNewIngredient += InitializeIngredientCard;
        mediator.PlayerHand.OnNewPotion += InitializePotionCard;
        mediator.PlayerCombat.OnSpawn += (Combatant combatant) => InitializePlayer((PlayerCombat)combatant);
        mediator.Horde.OnNewEnemy += InitializeEnemy;
    }

    private void Start()
    {
        BM.Instance.OnPlayerTurnStarted += ToggleHand;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ToggleHand();
        }
    }

    private void InitializeUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        root.Clear();

        foreach (StyleSheet sheet in styleSheet.styles)
            root.styleSheets.Add(sheet);

        var canvas = UITK.AddElement(root, "canvas", "MainText");

        hordeUI = UITK.AddElement(canvas, "hordeUI");

        handUI = UITK.AddElement(canvas, "handUI");

        interactionUI = UITK.AddElement(handUI, "interactionUI");

        bagUI = UITK.AddElement(handUI, "bagUI");

        beltUI = UITK.AddElement(handUI, "beltUI");



        var endTurnButton = UITK.AddElement<Button>(interactionUI, "endTurnButton", "MainButton");
        endTurnButton.style.backgroundImage = new StyleBackground(combatStyle.forward);
        endTurnButton.clicked += () =>
        {
            ClearCauldron();
            bagUI.Clear();
            ToggleHand();
            BM.Instance.CompletePlayerTurn();

            AudioManager.Instance.PlaySound(AudioManager.Instance.uiSounds[0]);
        };

        var brewButton = UITK.AddElement<Button>(interactionUI, "brewButton", "MainButton");
        brewButton.style.backgroundImage = new StyleBackground(combatStyle.cauldron);
        brewButton.clicked += BrewPotion;
    }

    private void InitializeIngredientCard(Ingredient_SO ingredient)
    {
        var ingredientCard = new IngredientCard(ingredient);
        ingredientCard.cardFrame.clicked += () => UseIngredientCard(ingredientCard);

        bagUI.Add(ingredientCard.cardFrame);

        ingredientCard.cardFrame.RegisterCallback<PointerEnterEvent>(evt =>
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[0]));
    }

    private void InitializePotionCard(Potion_SO potion)
    {
        if (potion == null) return;

        var potionCard = new PotionCard(potion);
        potionCard.cardFrame.clicked += () => UsePotionCard(potionCard);
       
        beltUI.Add(potionCard.cardFrame);

        potionCard.cardFrame.RegisterCallback<PointerEnterEvent>(evt =>
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[0]));
    }

    private void InitializeEnemy(Enemy enemy)
    {
        var enemyCard = new CombatantCard(enemy, combatStyle, mainCamera);
        hordeUI.Add(enemyCard.combatantFrame);
        enemyCard.combatantFrame.clicked += () => AttackEnemy(enemy);
    }

    private void InitializePlayer(PlayerCombat player)
    {
        var playerCard = new CombatantCard(player, combatStyle, mainCamera);
        hordeUI.Add(playerCard.combatantFrame);
    }

    private void UseIngredientCard(IngredientCard card)
    {
        if (!card.isSelected && cardsInCauldron.Count >= 3) return;

        if (card.Select())
        {
            cardsInCauldron.Add(card);
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[1]);
        }
        else
        {
            cardsInCauldron.Remove(card);
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[2]);
        }
    }

    private void UsePotionCard(PotionCard card)
    {
        if (card.potion is Elixir_SO elixir)
        {
            mediator.PlayerHand.UseElixir(elixir);
            card.cardFrame.RemoveFromHierarchy();
            AudioManager.Instance.PlaySound(AudioManager.Instance.potionSounds[1]);
            return;
        }

        if (card.potion is Flask_SO flask)
        {
            mediator.PlayerHand.UseFlask(flask);
            card.cardFrame.RemoveFromHierarchy();
            AudioManager.Instance.PlaySound(AudioManager.Instance.potionSounds[2]);
            return;
        }

        if (!card.isSelected && potionInUse != null)
            UsePotionCard(potionInUse);

        if (card.Select())
        {
            potionInUse = card;
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[1]);
        }
        else
        {
            potionInUse = null;
            AudioManager.Instance.PlaySound(AudioManager.Instance.cardSounds[2]);
        }
    }

    private void BrewPotion()
    {
        if (cardsInCauldron.Count < 3) return;
        if (beltUI.childCount >= 3) return;

        Ingredient_SO[] usedIngredients = cardsInCauldron.Select(card => card.ingredient).ToArray();

        mediator.PlayerHand.BrewNewPotion(usedIngredients);

        cardsInCauldron.ForEach(card => card.cardFrame.RemoveFromHierarchy());

        ClearCauldron();

        AudioManager.Instance.PlaySound(AudioManager.Instance.potionSounds[0]);
    }

    private void ClearCauldron()
    {
        cardsInCauldron = new List<IngredientCard>(0);
    }

    private void AttackEnemy(Enemy enemy)
    {
        if (potionInUse?.potion is Capsule_SO capsule)
        {
            mediator.PlayerHand.UseCapsule(capsule, enemy);
            potionInUse.cardFrame.RemoveFromHierarchy();
            potionInUse = null;
            AudioManager.Instance.PlaySound(AudioManager.Instance.potionSounds[3]);
        }
    }

    private void ToggleHand()
    {
        if (isHandVisible)
        {
            handUI.SetEnabled(false);
            handUI.pickingMode = PickingMode.Ignore;
            isHandVisible = false;
        }
        else
        {
            handUI.SetEnabled(true);
            handUI.pickingMode = PickingMode.Position;
            isHandVisible = true;
        }
    }
}
