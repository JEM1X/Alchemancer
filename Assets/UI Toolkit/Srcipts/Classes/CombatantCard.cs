using UnityEngine;
using UnityEngine.UIElements;

public class CombatantCard
{
    public Combatant combatant;
    public Button combatantFrame;

    private ProgressBar healthBar;
    private VisualElement healthFrame;
    private Label healthAmount;

    //Buff and Debuffs
    private VisualElement vulnerableFrame;
    private Label vulnerableAmount;
    private VisualElement resilientFrame;
    private Label resilientAmount;
    private VisualElement weakFrame;
    private Label weakAmount;
    private VisualElement strongFrame;
    private Label strongAmount;
    private VisualElement bleedFrame;
    private Label bleedAmount;
    private VisualElement stunFrame;
    private Label stunAmount;

    private UICombatStyle_SO combatantStyle;


    public CombatantCard(Combatant combatant, UICombatStyle_SO enemyStyle, Camera mainCamera)
    {
        this.combatant = combatant;
        this.combatantStyle = enemyStyle;
        InitializeEnemy(mainCamera);
    }

    public void InitializeEnemy(Camera mainCamera)
    {
        combatantFrame = UITK.CreateElement<Button>("enemyFrame");
        UpdateFramePos(mainCamera);

        var statusPanel = UITK.AddElement(combatantFrame, "statusPanel");

        var effectsPanel = UITK.AddElement(statusPanel, "effectsPanel");

        healthBar = UITK.AddElement<ProgressBar>(statusPanel, "healthBar");
        healthBar.highValue = combatant.HealthMax;

        healthFrame = UITK.AddElement(healthBar, "healthFrame", "EffectFrame");
        healthFrame.style.backgroundImage = new StyleBackground(combatantStyle.healthIcon);

        healthAmount = UITK.AddElement<Label>(healthFrame, "healthAmount", "EffectAmount", "ClearText");

        UpdateHealth(0);

        //vulnerable
        vulnerableFrame = UITK.AddElement(effectsPanel, "vulnerableFrame", "EffectFrame");
        vulnerableFrame.style.backgroundImage = new StyleBackground(combatantStyle.vulnerableIcon);

        vulnerableAmount = UITK.AddElement<Label>(vulnerableFrame, "vulnerableAmount", "EffectAmount", "ClearText");

        //resilient
        resilientFrame = UITK.AddElement(effectsPanel, "resilientFrame", "EffectFrame");
        resilientFrame.style.backgroundImage = new StyleBackground(combatantStyle.resilientIcon);

        resilientAmount = UITK.AddElement<Label>(resilientFrame, "resilientAmount", "EffectAmount", "ClearText");

        UpdateVulnerableResilient(0);

        //weak
        weakFrame = UITK.AddElement(effectsPanel, "weakFrame", "EffectFrame");
        weakFrame.style.backgroundImage = new StyleBackground(combatantStyle.weakIcon);

        weakAmount = UITK.AddElement<Label>(weakFrame, "weakAmount", "EffectAmount", "ClearText");

        //strong
        strongFrame = UITK.AddElement(effectsPanel, "strongFrame", "EffectFrame");
        strongFrame.style.backgroundImage = new StyleBackground(combatantStyle.weakIcon);

        strongAmount = UITK.AddElement<Label>(strongFrame, "strongAmount", "EffectAmount", "ClearText");

        UpdateWeakStrong(0);

        //bleed
        bleedFrame = UITK.AddElement(effectsPanel, "bleedFrame", "EffectFrame");
        bleedFrame.style.backgroundImage = new StyleBackground(combatantStyle.bleedIcon);

        bleedAmount = UITK.AddElement<Label>(bleedFrame, "bleedAmount", "EffectAmount", "ClearText");

        UpdateBleed(0);

        //stun
        stunFrame = UITK.AddElement(effectsPanel, "stunFrame", "effectFrame");
        stunFrame.style.backgroundImage = new StyleBackground(combatantStyle.stunIcon);

        stunAmount = UITK.AddElement<Label>(stunFrame, "stunAmount", "effectAmount", "ClearText");

        UpdateStun(0);

        //Events
        combatant.OnHealthChange += UpdateHealth;
        combatant.OnDeath += EnemyDeath;
        combatant.OnVulnerableResilientChange += UpdateVulnerableResilient;
        combatant.OnWeakStrongChange += UpdateWeakStrong;
        combatant.OnBleedChanged += UpdateBleed;
        combatant.OnStunChanged += UpdateStun;
    }

    private void UpdateFramePos(Camera mainCamera)
    {
        Vector2 enemyScreenPos = mainCamera.WorldToScreenPoint(combatant.transform.position);
        Vector2 framePos = new Vector2(enemyScreenPos.x, Screen.height - enemyScreenPos.y);

        combatantFrame.style.top = framePos.y - 150 - 250;
        combatantFrame.style.left = framePos.x - 980 - 100;
    }

    private void UpdateHealth(int amount)
    {
        healthBar.value = combatant.Health;
        healthAmount.text = combatant.Health.ToString();
    }

    private void UpdateVulnerableResilient(int amount)
    {
        int vulRes = combatant.VulnerableResilient;
        if (vulRes == 0)
        {
            vulnerableFrame.style.display = DisplayStyle.None;
            resilientFrame.style.display = DisplayStyle.None;
        }

        if (vulRes < 0)
        {
            resilientFrame.style.display = DisplayStyle.None;

            vulnerableFrame.style.display = DisplayStyle.Flex;
            vulnerableAmount.text = (-vulRes).ToString();
        }

        if(vulRes > 0)
        {
            vulnerableFrame.style.display = DisplayStyle.None;

            resilientFrame.style.display = DisplayStyle.Flex;
            resilientAmount.text = vulRes.ToString();
        }
    }

    private void UpdateWeakStrong(int amount)
    {
        int weakStrong = combatant.WeakStrong;

        if (weakStrong == 0)
        {
            weakFrame.style.display = DisplayStyle.None;
            strongFrame.style.display = DisplayStyle.None;
        }

        if (weakStrong < 0)
        {
            strongFrame.style.display = DisplayStyle.None;

            weakFrame.style.display = DisplayStyle.Flex;
            weakAmount.text = (-weakStrong).ToString();
        }

        if (weakStrong > 0)
        {
            weakFrame.style.display = DisplayStyle.None;

            strongFrame.style.display = DisplayStyle.Flex;
            strongAmount.text = weakStrong.ToString();
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

    private void EnemyDeath()
    {
        combatantFrame.RemoveFromHierarchy();
    }
}
