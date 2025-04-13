using System;
using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [Header("Attack Animation")]
    [SerializeField] private float actionDuration = 0.5f;
    [SerializeField] private Vector3 targetPos = new Vector3(-3, -1, 0);
    [SerializeField] private Vector3 castScale = new Vector3(1.2f, 1.2f, 1);
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private int _scorePoints;

    public EnemyAction_SO PlannedAction { get => plannedAction; }
    private EnemyAction_SO plannedAction;
    [SerializeField] private EnemyAction_SO[] enemyActions;

    public static event Action<int> OnScoreGain;
    public event Action OnNewPlannedAttack;


    protected override IEnumerator Action()
    {
        plannedAction.ExecuteAction(this);
        yield return new WaitForSeconds(actionDuration);
    }

    public IEnumerator AttackLunge()
    {
        Vector3 startPos = transform.position;;

        // In
        float duration = 0;
        while (duration < 1)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, Easing.InCubic(duration));
            duration += Time.deltaTime / (actionDuration / 2);
            yield return null;
        }

        // Out
        duration = 0;
        while (duration < 1)
        {
            transform.position = Vector3.Lerp(targetPos, startPos, Easing.OutCubic(duration));
            duration += Time.deltaTime / (actionDuration / 2);
            yield return null;
        }

        transform.position = startPos;
    }

    public IEnumerator SelfCast()
    {
        Vector3 startScale = transform.localScale;

        // Up
        float duration = 0;
        while (duration < 1)
        {
            transform.localScale = Vector3.Lerp(startScale, castScale, Easing.InCubic(duration));
            duration += Time.deltaTime / (actionDuration / 2);
            yield return null;
        }

        // Down
        duration = 0;
        while (duration < 1)
        {
            transform.localScale = Vector3.Lerp(castScale, startScale, Easing.OutCubic(duration));
            duration += Time.deltaTime / (actionDuration / 2);
            yield return null;
        }

        transform.localScale = startScale;
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
