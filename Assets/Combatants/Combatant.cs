using UnityEngine;
using System;
using System.Collections;

public abstract class Combatant : MonoBehaviour
{
    public int size = 200;

    public int HealthMax { get => healthMax; }
    [SerializeField] protected int healthMax = 5;
    public int Health { get => health; }
    [SerializeField] protected int health = 5;

    [SerializeField] private int power;

    [SerializeField] private int influence;

    public int VulnerableResilient { get => vulnerableResilient; }
    [SerializeField] protected int vulnerableResilient = 0;
    public int WeakStrong { get => weakStrong; }
    [SerializeField] protected int weakStrong = 0;
    public int DullBright { get => dullBright; }
    [SerializeField] protected int dullBright = 0;
    public int Bleed { get => bleed; }
    [SerializeField] protected int bleed = 0;
    public int Stun { get => stun; }
    [SerializeField] protected int stun = 0;

    private Animator _animator;

    public event Action<Combatant> OnSpawn;
    public event Action OnTurnStart;
    public event Action OnActionStart;
    public event Action OnTurnEnd;
    public event Action OnDeath;
    public event Action<int> OnHealthChange;
    public event Action<int> OnVulnerableResilientChange;
    public event Action<int> OnWeakStrongChange;
    public event Action<int> OnDullBrightChange;
    public event Action<int> OnBleedChanged;
    public event Action<int> OnStunChanged;

    private event Action CompleteTurn;

    public int Power
    {
        get
        {
            if (weakStrong == 0)
                return power;

            return weakStrong > 0 ? power * 2 : power / 2;
        }
    }

    public int Influence
    {
        get
        {
            if (dullBright == 0)
                return influence;

            return dullBright > 0 ? influence * 2 : influence / 2;
        }
    }


    protected void Start()
    {
        OnSpawn?.Invoke(this);
    }

    public virtual IEnumerator TakeTurn(Action CompleteTurn)
    {
        this.CompleteTurn = CompleteTurn;

        OnTurnStart?.Invoke();

        //pre-action effects
        ReduceBleed();

        ReduceVulnerableResilient();

        if (ReduceStun())
        {
            ReduceWeakStrong();

            OnTurnEnd?.Invoke();
            CompleteTurn?.Invoke();
            yield break;
        }

        //action
        OnActionStart?.Invoke();
        yield return StartCoroutine(Action());

        //post-action effects
        ReduceWeakStrong();

        ReduceDullBright(); 

        OnTurnEnd?.Invoke();

        CompleteTurn?.Invoke();
    }

    protected abstract IEnumerator Action();

    protected virtual void Death()
    {
        CompleteTurn?.Invoke();
        OnDeath?.Invoke();
        StopAllCoroutines();
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
        OnStunChanged = null;
        CompleteTurn = null;
        OnDeath = null;
    }

    public void TakeDamage(int damage)
    {
        if (vulnerableResilient < 0)
            damage *= 2;

        if (vulnerableResilient > 0)
            damage = damage / 2;

        TakeTrueDamage(damage);
    }

    public void TakeTrueDamage(int damage)
    {
        health -= damage;
        OnHealthChange?.Invoke(-damage);

        if (health <= 0)
        {
            Death();
        }
    }

    public void TakeHeal(int heal)
    {
        int lostHealth = healthMax - health;
        if (lostHealth < heal)
            heal = lostHealth;

        health += heal;
        OnHealthChange?.Invoke(heal);
    }

    public void InflictVulnerableResilient(int amount)
    {
        vulnerableResilient += amount;
        OnVulnerableResilientChange?.Invoke(amount);
    }

    public void InflictWeakStrong(int amount)
    {
        weakStrong += amount;
        OnWeakStrongChange?.Invoke(amount);
    }

    public void InflictDullBright(int amount)
    {
        dullBright += amount;
        OnDullBrightChange?.Invoke(amount);
    }

    public void InflictBleed(int amount)
    {
        bleed += amount;
        OnBleedChanged?.Invoke(amount);
    }

    public void InflictStun(int amount)
    {
        stun += amount;
        OnStunChanged?.Invoke(amount);
    }

    private void ReduceVulnerableResilient()
    {
        if (vulnerableResilient == 0) return;

        int change = vulnerableResilient > 0 ? -1 : 1;
        vulnerableResilient += change;

        OnVulnerableResilientChange?.Invoke(change);
    }

    private void ReduceWeakStrong()
    {
        if (weakStrong == 0) return;

        int change = weakStrong > 0 ? -1 : 1;
        weakStrong += change;

        OnWeakStrongChange?.Invoke(change);
    }

    private void ReduceDullBright()
    {
        if (dullBright == 0) return;

        int change = dullBright > 0 ? -1 : 1;
        dullBright += change;

        OnDullBrightChange?.Invoke(change);
    }

    private void ReduceBleed()
    {
        if (bleed <= 0) return;
        
        int bleedDamage = Mathf.Max(1, bleed / 2);
        TakeTrueDamage(bleedDamage);
        bleed = Mathf.Max(0, bleed - bleedDamage);

        OnBleedChanged?.Invoke(bleed);
    }

    private bool ReduceStun()
    {
        if (stun <= 0) return false;

        stun -= 1;
        OnStunChanged?.Invoke(-1);

        return true;
    }
}
