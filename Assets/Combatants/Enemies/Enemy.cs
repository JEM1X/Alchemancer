using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [Header("Attack Animation")]
    [SerializeField] private float attackMoveDistance = 0.5f;
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private int damage;


    public void TakeTurn(System.Action onTurnEnd)
    {
        StartCoroutine(AttackSequence(onTurnEnd));
    }

    private IEnumerator AttackSequence(System.Action onTurnEnd)
    {
        yield return StartCoroutine(AttackLunge());

        if (attackParticles != null)
            attackParticles.Play();
        if (weakStrong != 0)
        {
            BattleManager.Instance.Player.TakeDamage(damage / 2);
        }
        else 
        {
            BattleManager.Instance.Player.TakeDamage(damage);
        }

        onTurnEnd?.Invoke();
    }

    private IEnumerator AttackLunge()
    {
        Vector3 startPos = transform.position;

        // Направление атаки зависит от флипа по X
        float direction = -Mathf.Sign(transform.localScale.x);
        Vector3 attackPos = startPos + new Vector3(attackMoveDistance * direction, 0f, 0f);

        // Движение вперёд
        float time = 0;
        while (time < attackDuration / 2)
        {
            transform.position = Vector3.Lerp(startPos, attackPos, time / (attackDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        // Возврат назад
        time = 0;
        while (time < attackDuration / 2)
        {
            transform.position = Vector3.Lerp(attackPos, startPos, time / (attackDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
    }

    protected override void Death()
    {
        StopAllCoroutines();
        base.Death();
        Destroy(gameObject);
    }
}
