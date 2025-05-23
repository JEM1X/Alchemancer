using UnityEngine;
using UnityEngine.UIElements;

public class CombatantCard
{
    public Combatant combatant;
    public Button combatantFrame;

    private ProgressBar healthBar;
    private VisualElement healthFrame;
    private Label healthAmount;
    private VisualElement powerFrame;
    private Label powerAmount;
    private VisualElement influenceFrame;
    private Label influenceAmount;

    //Buff and Debuffs
    private VisualElement vulnerableFrame;
    private Label vulnerableAmount;
    private VisualElement resilientFrame;
    private Label resilientAmount;
    private VisualElement weakFrame;
    private Label weakAmount;
    private VisualElement strongFrame;
    private Label strongAmount;
    private VisualElement dullFrame;
    private Label dullAmount;
    private VisualElement brightFrame;
    private Label brightAmount;
    private VisualElement bleedFrame;
    private Label bleedAmount;
    private VisualElement stunFrame;
    private Label stunAmount;
    private VisualElement dodgeFrame;
    private Label dodgeAmount;

    private UICombatStyle_SO combatantStyle;


    public CombatantCard(Combatant combatant, UICombatStyle_SO enemyStyle, Camera mainCamera)
    {
        this.combatant = combatant;
        this.combatantStyle = enemyStyle;
        InitializeCombatant(mainCamera);
    }

    private void InitializeCombatant(Camera mainCamera)
    {
        combatantFrame = UITK.CreateElement<Button>("combatantFrame");
        combatantFrame.style.height = combatant.size;
        SetFramePos(mainCamera);

        var statusPanel = UITK.AddElement(combatantFrame, "statusPanel");

        var effectsPanel = UITK.AddElement(statusPanel, "effectsPanel");

        //healthBar
        healthBar = UITK.AddElement<ProgressBar>(statusPanel, "healthBar");
        healthBar.highValue = combatant.HealthMax;

        //influence
        influenceFrame = UITK.AddElement(healthBar, "StatFrame", "EffectFrame");
        influenceFrame.style.backgroundImage = new StyleBackground(combatantStyle.influenceIcon);

        influenceAmount = UITK.AddElement<Label>(influenceFrame, "influenceAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(influenceFrame, UITK.UITABLE, "Combat.Hint.Influence");
        StatsHoverAnimation(influenceFrame, 200);
        UpdateInfluence();

        //power
        powerFrame = UITK.AddElement(healthBar, "StatFrame", "EffectFrame");
        powerFrame.style.backgroundImage = new StyleBackground(combatantStyle.powerIcon);

        powerAmount = UITK.AddElement<Label>(powerFrame, "powerAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(powerFrame, UITK.UITABLE, "Combat.Hint.Power");
        StatsHoverAnimation(powerFrame, 100);
        UpdatePower();

        //health
        healthFrame = UITK.AddElement(healthBar, "healthFrame", "EffectFrame");
        healthFrame.style.backgroundImage = new StyleBackground(combatantStyle.healthIcon);

        healthAmount = UITK.AddElement<Label>(healthFrame, "healthAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(healthFrame, UITK.UITABLE, "Combat.Hint.Health");
        UpdateHealth(0);

        //vulnerable
        vulnerableFrame = UITK.AddElement(effectsPanel, "vulnerableFrame", "EffectFrame");
        vulnerableFrame.style.backgroundImage = new StyleBackground(combatantStyle.vulnerableIcon);

        vulnerableAmount = UITK.AddElement<Label>(vulnerableFrame, "vulnerableAmount", "EffectAmount", "ClearText");

        //resilient
        resilientFrame = UITK.AddElement(effectsPanel, "resilientFrame", "EffectFrame");
        resilientFrame.style.backgroundImage = new StyleBackground(combatantStyle.resilientIcon);

        resilientAmount = UITK.AddElement<Label>(resilientFrame, "resilientAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(vulnerableFrame, UITK.UITABLE, "Combat.Hint.Vulnerability");
        UIMenu.AddLocalizedHintBox(resilientFrame, UITK.UITABLE, "Combat.Hint.Resilience");
        UpdateVulnerableResilient(0);

        //weak
        weakFrame = UITK.AddElement(effectsPanel, "weakFrame", "EffectFrame");
        weakFrame.style.backgroundImage = new StyleBackground(combatantStyle.weakIcon);

        weakAmount = UITK.AddElement<Label>(weakFrame, "weakAmount", "EffectAmount", "ClearText");

        //strong
        strongFrame = UITK.AddElement(effectsPanel, "strongFrame", "EffectFrame");
        strongFrame.style.backgroundImage = new StyleBackground(combatantStyle.strongIcon);

        strongAmount = UITK.AddElement<Label>(strongFrame, "strongAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(weakFrame, UITK.UITABLE, "Combat.Hint.Weakness");
        UIMenu.AddLocalizedHintBox(strongFrame, UITK.UITABLE, "Combat.Hint.Strength");
        UpdateWeakStrong(0);

        //dull
        dullFrame = UITK.AddElement(effectsPanel, "dullFrame", "EffectFrame");
        dullFrame.style.backgroundImage = new StyleBackground(combatantStyle.dullIcon);

        dullAmount = UITK.AddElement<Label>(dullFrame, "weakAmount", "EffectAmount", "ClearText");

        //bright
        brightFrame = UITK.AddElement(effectsPanel, "brightFrame", "EffectFrame");
        brightFrame.style.backgroundImage = new StyleBackground(combatantStyle.brightIcon);

        brightAmount = UITK.AddElement<Label>(brightFrame, "strongAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(dullFrame, UITK.UITABLE, "Combat.Hint.Dull");
        UIMenu.AddLocalizedHintBox(brightFrame, UITK.UITABLE, "Combat.Hint.Bright");
        UpdateDullBright(0);

        //bleed
        bleedFrame = UITK.AddElement(effectsPanel, "bleedFrame", "EffectFrame");
        bleedFrame.style.backgroundImage = new StyleBackground(combatantStyle.bleedIcon);

        bleedAmount = UITK.AddElement<Label>(bleedFrame, "bleedAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(bleedFrame, UITK.UITABLE, "Combat.Hint.Bleed");
        UpdateBleed(0);

        //stun
        stunFrame = UITK.AddElement(effectsPanel, "stunFrame", "EffectFrame");
        stunFrame.style.backgroundImage = new StyleBackground(combatantStyle.stunIcon);

        stunAmount = UITK.AddElement<Label>(stunFrame, "stunAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(stunFrame, UITK.UITABLE, "Combat.Hint.Stun");
        UpdateStun(0);

        //dodge
        dodgeFrame = UITK.AddElement(effectsPanel, "dodgeFrame", "EffectFrame");
        dodgeFrame.style.backgroundImage = new StyleBackground(combatantStyle.dodgeIcon);

        dodgeAmount = UITK.AddElement<Label>(dodgeFrame, "dodgeAmount", "EffectAmount", "ClearText");

        UIMenu.AddLocalizedHintBox(dodgeFrame, UITK.UITABLE, "Combat.Hint.Dodge");
        UpdateDodge(0);

        //Events
        combatant.OnDeath += EnemyDeath;
        combatant.OnHealthChange += UpdateHealth;
        combatant.OnWeakStrongChange += (int amount) => UpdatePower();
        combatant.OnDullBrightChange += (int amount) => UpdateInfluence();
        combatant.OnVulnerableResilientChange += UpdateVulnerableResilient;
        combatant.OnWeakStrongChange += UpdateWeakStrong;
        combatant.OnDullBrightChange += UpdateDullBright;
        combatant.OnBleedChange += UpdateBleed;
        combatant.OnStunChange += UpdateStun;
        combatant.OnDodgeChange += UpdateDodge;
    }

    private void SetFramePos(Camera mainCamera)
    {
        Vector2 enemyScreenPos = mainCamera.WorldToScreenPoint(combatant.transform.position);
        Vector2 framePos = new Vector2(enemyScreenPos.x, Screen.height - enemyScreenPos.y);

        combatantFrame.style.top = framePos.y - (Screen.height / 4) - combatant.size + 50;
        combatantFrame.style.left = framePos.x;
    }

    private void UpdateHealth(int amount)
    {
        healthBar.value = combatant.Health;
        healthAmount.text = combatant.Health.ToString();
    }

    private void UpdatePower()
    {
        powerAmount.text = combatant.Power.ToString();
    }

    private void UpdateInfluence()
    {
        influenceAmount.text = combatant.Influence.ToString();
    }

    private void UpdateVulnerableResilient(int amount)
    {
        int vulRes = combatant.VulnerableResilient;
        
        vulnerableFrame.style.display = DisplayStyle.None;
        resilientFrame.style.display = DisplayStyle.None;

        if (vulRes < 0)
        {
            vulnerableFrame.style.display = DisplayStyle.Flex;
            vulnerableAmount.text = (-vulRes).ToString();
        }
        else if(vulRes > 0)
        {
            resilientFrame.style.display = DisplayStyle.Flex;
            resilientAmount.text = vulRes.ToString();
        }
    }

    private void UpdateWeakStrong(int amount)
    {
        int weakStrong = combatant.WeakStrong;

        weakFrame.style.display = DisplayStyle.None;
        strongFrame.style.display = DisplayStyle.None;

        if (weakStrong < 0)
        {
            weakFrame.style.display = DisplayStyle.Flex;
            weakAmount.text = (-weakStrong).ToString();
        }
        else if (weakStrong > 0)
        {
            strongFrame.style.display = DisplayStyle.Flex;
            strongAmount.text = weakStrong.ToString();
        }
    }

    private void UpdateDullBright(int amount)
    {
        int dullBright = combatant.DullBright;

        dullFrame.style.display = DisplayStyle.None;
        brightFrame.style.display = DisplayStyle.None;

        if (dullBright < 0)
        {
            dullFrame.style.display = DisplayStyle.Flex;
            dullAmount.text = (-dullBright).ToString();
        }
        else if (dullBright > 0)
        {
            brightFrame.style.display = DisplayStyle.Flex;
            brightAmount.text = dullBright.ToString();
        }
    }

    private void UpdateBleed(int amount)
    {
        int bleed = combatant.Bleed;

        if(bleed <= 0)
        {
            bleedFrame.style.display = DisplayStyle.None;
        }

        if(bleed > 0)
        {
            bleedFrame.style.display = DisplayStyle.Flex;
            bleedAmount.text = bleed.ToString();
        }
    }

    private void UpdateStun(int amount)
    {
        int stun = combatant.Stun;

        if (stun <= 0)
        {
            stunFrame.style.display = DisplayStyle.None;
        }

        if (stun > 0)
        {
            stunFrame.style.display = DisplayStyle.Flex;
            stunAmount.text = stun.ToString();
        }
    }

    private void UpdateDodge(int amount)
    {
        int dodge = combatant.Dodge;

        if (dodge <= 0)
        {
            dodgeFrame.style.display = DisplayStyle.None;
        }

        if (dodge > 0)
        {
            dodgeFrame.style.display = DisplayStyle.Flex;
            dodgeAmount.text = dodge.ToString();
        }
    }

    private void EnemyDeath()
    {
        combatantFrame.RemoveFromHierarchy();
    }

    private void StatsHoverAnimation(VisualElement element, int pos)
    {
        combatantFrame.RegisterCallback<PointerEnterEvent>(evt =>
        {
            StyleTranslate newPos = new StyleTranslate(new Translate(new Length(pos), new Length(0)));
            element.style.translate = newPos;
            element.style.opacity = 100;
        });

        combatantFrame.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            element.style.translate = StyleKeyword.Null;
            element.style.opacity = StyleKeyword.Null;
        });
    }
}
