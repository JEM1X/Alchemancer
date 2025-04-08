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
    public int Bleed { get => bleed; }
    [SerializeField] protected int bleed = 0;
    public int Stun { get => stun; }
    [SerializeField] protected int stun = 0; 

    private Animator _animator;

    public event Action<Combatant> OnSpawn;
    public event Action OnTurnStart;
    public event Action OnTurnEnd;
    public event Action OnDeath;
    public event Action<int> OnHealthChange;
    public event Action<int> OnVulnerableResilientChange;
    public event Action<int> OnWeakStrongChange;
    public event Action<int> OnBleedChanged;
    public event Action<int> OnStunChanged;

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

        //pre-attack effects
        ReduceBleed();

        if (ReduceStun())
        {
            ReduceWeakStrong();
            ReduceVulnerableResilient();

            OnTurnEnd?.Invoke();
            CompleteTurn?.Invoke();
            yield break;
        }

        //Attack
        yield return StartCoroutine(Attack());

        //post-attack effects
        ReduceWeakStrong();

        ReduceVulnerableResilient();

        OnTurnEnd?.Invoke();

        CompleteTurn?.Invoke();
    }

    protected abstract IEnumerator Attack();

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

    private void ReduceBleed()
    {
        if (bleed <= 0) return;
        
        int bleedDamage = Mathf.Max(1, bleed / 2);
        TakeDamage(bleedDamage);
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
}
