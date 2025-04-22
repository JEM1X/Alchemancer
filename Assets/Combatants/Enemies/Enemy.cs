using System;
using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [SerializeField] private int scorePoints;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLibraire audioLibraire;

    [Header("Animations")]
    [SerializeField] private float actionDuration = 0.6f;
    [SerializeField] private Vector3 targetPos = new Vector3(-3, -1, 0);
    [SerializeField] private Vector3 castScale = new Vector3(1.1f, 1.1f, 1);
    [SerializeField] private float impactDuration = 0.3f;
    [SerializeField] private Vector3 attackImpactPos = new Vector3(0.5f, 0, 0);
    [SerializeField] private Vector3 castImpactScale = new Vector3(0.9f, 0.9f, 1);
    [SerializeField] private ParticleSystem attackParticles;

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

    public void PlanNextAction()
    {
        plannedAction = enemyActions[UnityEngine.Random.Range(0, enemyActions.Length)];
        OnNewPlannedAttack?.Invoke();
    }

    protected override void Death()
    {
        base.Death();
        OnScoreGain?.Invoke(scorePoints);
        Destroy(gameObject);
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

        AudioClip randomPunch = audioLibraire.punchSounds[UnityEngine.Random.Range(0, audioLibraire.punchSounds.Length)];
        audioSource.PlayOneShot(randomPunch);

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

    public IEnumerator AttackImpact()
    {
        Vector3 startPos = transform.position; ;

        // In
        float duration = 0;
        while (duration < 1)
        {
            transform.position = Vector3.Lerp(startPos, startPos + attackImpactPos, Easing.OutCubic(duration));
            duration += Time.deltaTime / (impactDuration / 2);
            yield return null;
        }

        // Out
        duration = 0;
        while (duration < 1)
        {
            transform.position = Vector3.Lerp(startPos + attackImpactPos, startPos, Easing.InOutCubic(duration));
            duration += Time.deltaTime / (impactDuration / 2);
            yield return null;
        }

        transform.position = startPos;
    }

    public IEnumerator CastImpact()
    {
        Vector3 startScale = transform.localScale;

        // Up
        float duration = 0;
        while (duration < 1)
        {
            transform.localScale = Vector3.Lerp(startScale, castImpactScale, Easing.OutCubic(duration));
            duration += Time.deltaTime / (impactDuration / 2);
            yield return null;
        }

        // Down
        duration = 0;
        while (duration < 1)
        {
            transform.localScale = Vector3.Lerp(castImpactScale, startScale, Easing.InOutCubic(duration));
            duration += Time.deltaTime / (impactDuration / 2);
            yield return null;
        }

        transform.localScale = startScale;
    }
}
