using System;
using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [Header("Attack Animation")]
    [SerializeField] private float attackMoveDistance = 0.5f;
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private int _scorePoints;

    public EnemyAction_SO PlannedAction { get => plannedAction; }
    private EnemyAction_SO plannedAction;
    [SerializeField] private EnemyAction_SO[] enemyActions;

    public static event Action<int> OnScoreGain;
    public event Action OnNewPlannedAttack;


    protected override IEnumerator Action()
    {
        yield return StartCoroutine(AttackLunge());
        plannedAction.ExecuteAction(this);
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

    public void PlanNextAction()
    {
        plannedAction = enemyActions[UnityEngine.Random.Range(0, enemyActions.Length)];
        OnNewPlannedAttack?.Invoke();
    }

    protected override void Death()
    {
        base.Death();
        OnScoreGain?.Invoke(_scorePoints);
        Destroy(gameObject);
    }
}
