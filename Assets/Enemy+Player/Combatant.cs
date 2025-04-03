using UnityEngine;
using System;

public abstract class Combatant : MonoBehaviour
{
    public int HealthMax { get => healthMax; }
    [SerializeField] private int healthMax = 5;
    public int Health { get => health; }
    [SerializeField] private int health = 5;

    public event Action<int> OnHealthChange;
    public event Action OnDeath;


    public virtual void TakeDamage(int damage)
    {
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

    protected virtual void Death()
    {
        OnDeath?.Invoke();
        //Destroy(gameObject);
    }
}
