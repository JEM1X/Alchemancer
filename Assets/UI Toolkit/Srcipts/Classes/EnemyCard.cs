using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCard : CombatantCard
{
    public Enemy enemy;

    private VisualElement enemyAttack;


    public EnemyCard(Enemy enemy, UICombatStyle_SO enemyStyle, Camera mainCamera) : base(enemy, enemyStyle, mainCamera)
    {
        this.enemy = enemy;
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        enemyAttack = UITK.AddElement(combatantFrame, "enemyAttack");

        enemy.OnNewPlannedAttack += UpdateAttack;
    }

    private void UpdateAttack()
    {
        enemyAttack.style.backgroundImage = new StyleBackground(enemy.PlannedAction.ActionIcon);
    }
}
