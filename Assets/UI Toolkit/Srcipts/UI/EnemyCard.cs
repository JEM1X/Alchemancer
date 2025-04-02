using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCard
{
    public Enemy enemy;
    public Button enemyFrame;

    private UIEnemyStyle_SO enemyStyle;

    public EnemyCard(Enemy enemy, UIEnemyStyle_SO enemyStyle, Camera mainCamera)
    {
        this.enemy = enemy;
        this.enemyStyle = enemyStyle;
        InitializeEnemy(mainCamera);
    }

    public void InitializeEnemy(Camera mainCamera)
    {
        enemyFrame = UITK.CreateElement<Button>("enemyFrame");
        UpdateFramePos(mainCamera);

        var healthFrame = UITK.AddElement(enemyFrame, "healthFrame");
        healthFrame.style.backgroundImage = new StyleBackground(enemyStyle.healthIcon);

        var healthAmount = UITK.AddElement<Label>(healthFrame, "healthAmount");
        healthAmount.text = enemy.Health.ToString();
    }

    private void UpdateFramePos(Camera mainCamera)
    {
        Vector2 enemyScreenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
        Vector2 framePos = new Vector2(enemyScreenPos.x, Screen.height - enemyScreenPos.y);

        enemyFrame.style.top = framePos.y - 150 - 120;
        enemyFrame.style.left = framePos.x - 980 - 70;
    }
}
