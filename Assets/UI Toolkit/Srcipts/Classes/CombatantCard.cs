using UnityEngine;
using UnityEngine.UIElements;

public class CombatantCard
{
    public Combatant combatant;
    public Button combatantFrame;

    //Buff and Debuffs
    public VisualElement vulnerableFrame;
    public Label vulnerableAmount;
    public VisualElement resilientFrame;
    public Label resilientAmount;
    public VisualElement weakFrame;
    public Label weakAmount;
    public VisualElement strongFrame;
    public Label strongAmount;


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

        var healthFrame = UITK.AddElement(combatantFrame, "healthFrame", "effectFrame");
        healthFrame.style.backgroundImage = new StyleBackground(combatantStyle.healthIcon);

        var healthAmount = UITK.AddElement<Label>(healthFrame, "healthAmount", "effectAmount", "ClearText");
        healthAmount.text = combatant.Health.ToString();

        //vulnerable
        vulnerableFrame = UITK.AddElement(combatantFrame, "vulnerableFrame", "effectFrame");
        vulnerableFrame.style.backgroundImage = new StyleBackground(combatantStyle.vulnerableIcon);

        vulnerableAmount = UITK.AddElement<Label>(vulnerableFrame, "vulnerableAmount", "effectAmount", "ClearText");

        //resilient
        resilientFrame = UITK.AddElement(combatantFrame, "resilientFrame", "effectFrame");
        resilientFrame.style.backgroundImage = new StyleBackground(combatantStyle.resilientIcon);

        resilientAmount = UITK.AddElement<Label>(resilientFrame, "resilientAmount", "effectAmount", "ClearText");

        UpdateVulnerableResilient(0);

        //weak
        weakFrame = UITK.AddElement(combatantFrame, "weakFrame", "effectFrame");
        weakFrame.style.backgroundImage = new StyleBackground(combatantStyle.weakIcon);

        weakAmount = UITK.AddElement<Label>(weakFrame, "weakAmount", "effectAmount", "ClearText");

        //strong
        strongFrame = UITK.AddElement(combatantFrame, "strongFrame", "effectFrame");
        strongFrame.style.backgroundImage = new StyleBackground(combatantStyle.weakIcon);

        strongAmount = UITK.AddElement<Label>(strongFrame, "strongAmount", "effectAmount", "ClearText");

        UpdateWeakStrong(0);

        //Events
        combatant.OnHealthChange += (int health) => healthAmount.text = combatant.Health.ToString();
        combatant.OnDeath += EnemyDeath;
        combatant.OnVulnerableResilientChange += UpdateVulnerableResilient;
        combatant.OnWeakStrongChange += UpdateWeakStrong;
    }

    private void UpdateFramePos(Camera mainCamera)
    {
        Vector2 enemyScreenPos = mainCamera.WorldToScreenPoint(combatant.transform.position);
        Vector2 framePos = new Vector2(enemyScreenPos.x, Screen.height - enemyScreenPos.y);

        combatantFrame.style.top = framePos.y - 150 - 200;
        combatantFrame.style.left = framePos.x - 980 - 75;
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
            weakAmount.text = combatant.WeakStrong.ToString();
        }

        if (weakStrong > 0)
        {
            weakFrame.style.display = DisplayStyle.None;

            strongFrame.style.display = DisplayStyle.Flex;
            strongAmount.text = combatant.WeakStrong.ToString();
        }
    }

    private void EnemyDeath()
    {
        combatantFrame.RemoveFromHierarchy();
    }
}
