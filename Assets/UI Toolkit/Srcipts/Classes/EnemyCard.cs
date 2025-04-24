using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCard : CombatantCard
{
    public Enemy enemy;

    private VisualElement actionIcon;
    private VisualElement actionSubIcon;
    private Label actionHint;


    public EnemyCard(Enemy enemy, UICombatStyle_SO enemyStyle, Camera mainCamera) : base(enemy, enemyStyle, mainCamera)
    {
        this.enemy = enemy;
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        actionIcon = UITK.AddElement(combatantFrame, "actionIcon");
        actionSubIcon = UITK.AddElement(actionIcon, "actionSubIcon");

        actionHint = UIMenu.AddHintBox(actionIcon);

        enemy.OnNewPlannedAttack += UpdateAttack;
    }

    private void UpdateAttack()
    {
        actionIcon.style.backgroundImage = new StyleBackground(enemy.PlannedAction.ActionIcon);
        actionSubIcon.style.backgroundImage = new StyleBackground(enemy.PlannedAction.SubIcon);
        UITK.LocalizeStringUITK(actionHint, UITK.UITABLE, enemy.PlannedAction.Description);
    }
}
