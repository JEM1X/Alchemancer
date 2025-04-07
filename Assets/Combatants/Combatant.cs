using UnityEngine;
using System;
using System.Collections;

public abstract class Combatant : MonoBehaviour
{
    public int HealthMax { get => healthMax; }
    [SerializeField] protected int healthMax = 5;
    public int Health { get => health; }
    [SerializeField] protected int health = 5;
    public int VulnerableResilient { get => vulnerableResilient; }
    [SerializeField] protected int vulnerableResilient = 0;
    public int WeakStrong { get => weakStrong; }
    [SerializeField] protected int weakStrong = 0;
    public int BleedStacks { get => bleedStacks; }
    [SerializeField] protected int bleedStacks = 0;

    private Animator _animator;

    public event Action<Combatant> OnSpawn;
    public event Action OnDeath;
    public event Action OnTurnStart;
    public event Action OnTurnEnd;
    public event Action<int> OnHealthChange;
    public event Action<int> OnVulnerableResilientChange;
    public event Action<int> OnWeakStrongChange;
    public event Action<int> OnBleedChanged;

    private event Action CompleteTurn;


    protected void Start()
    {
        //_animator = GetComponent<Animator>();
        OnSpawn?.Invoke(this);
    }

    public virtual IEnumerator TakeTurn(Action CompleteTurn)
    {
        this.CompleteTurn = CompleteTurn;

        OnTurnStart?.Invoke();
        BeforeTurnEffects();

        yield return StartCoroutine(Attack());

        AfterTurnEffects();
        OnTurnEnd?.Invoke();

        CompleteTurn?.Invoke();
    }

    protected abstract IEnumerator Attack();

    protected virtual void Death()
    {
        CompleteTurn?.Invoke();
        OnDeath?.Invoke();
        ClearAllListeners();

        //if (_animator != null) 
        //{
        //    _animator.SetBool("isDead", true);
        //}
        //Destroy(gameObject);
    }

    public void ClearAllListeners()
    {
        OnSpawn = null;
        OnHealthChange = null;
        OnVulnerableResilientChange = null;
        OnWeakStrongChange = null;
        OnBleedChanged = null;
        CompleteTurn = null;
        OnDeath = null;
    }

    public virtual void TakeDamage(int damage)
    {
        if (vulnerableResilient < 0)
            damage *= 2;

        if (vulnerableResilient > 0)
            damage = damage / 2;

        health -= damage;
        OnHealthChange?.Invoke(-damage);

        if (health <= 0)
        {
            Death();
        }
        Debug.Log(health);
    }

    public virtual void TakeHeal(int heal)
    {
        int lostHealth = healthMax - health;
        if (lostHealth < heal)
            heal = lostHealth;

        health += heal;
        OnHealthChange?.Invoke(heal);
    }

    public virtual void InflictBleed(int stacks)
    {
        bleedStacks += stacks;
        OnBleedChanged?.Invoke(bleedStacks);
    }

    public virtual void InflictVulnerableResilient(int amount)
    {
        vulnerableResilient += amount;
        OnVulnerableResilientChange?.Invoke(amount);
    }

    public virtual void InflictWeakStrong(int amount)
    {
        weakStrong += amount;
        OnWeakStrongChange?.Invoke(amount);
    }

    protected virtual void BeforeTurnEffects()
    {
        ReduceBleed();
    }

    protected virtual void AfterTurnEffects()
    {
        ReduceWeakStrong();

        ReduceVulnerableResilient();
    }

    private void ReduceBleed()
    {
        if (bleedStacks > 0)
        {
            int bleedDamage = Mathf.Max(1, bleedStacks / 2);
            TakeDamage(bleedDamage);
            bleedStacks = Mathf.Max(0, bleedStacks - bleedDamage);

            OnBleedChanged?.Invoke(bleedStacks);
        }
    }

    private void ReduceVulnerableResilient()
    {
        if (vulnerableResilient != 0)
        {
            int change = vulnerableResilient > 0 ? -1 : 1;
            vulnerableResilient += change;
            OnVulnerableResilientChange?.Invoke(change);
        }
    }

    private void ReduceWeakStrong()
    {
        if (weakStrong != 0)
        {
            int change = weakStrong > 0 ? -1 : 1;
            weakStrong += change;
            OnWeakStrongChange?.Invoke(change);
        }
    }
}
