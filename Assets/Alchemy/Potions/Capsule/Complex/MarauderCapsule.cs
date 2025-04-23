using UnityEngine;

[CreateAssetMenu(fileName = "MarauderCapsule", menuName = "Scriptable Objects/Potion/Capsule/Complex/MarauderCapsule")]
public class MarauderCapsule : Capsule_SO
{
    [SerializeField] private int cardsAmount = 2;

    public override void UseCapsule(Alchemancer user, Enemy enemy)
    {
        enemy.TakeDamage(user.PlayerCombat.Power);

        enemy.StartCoroutine(enemy.AttackImpact());

        if (enemy.Health <= 0)
        {
            for (int i = 0; i < cardsAmount; i++)
                user.PlayerHand.DrawRandomIngredient();
        }
    }
}
