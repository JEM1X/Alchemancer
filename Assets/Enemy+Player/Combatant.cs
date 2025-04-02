using UnityEngine;
using System;

public abstract class Combatant : MonoBehaviour
{
    public int Health { get; protected set; }

    public event Action<int> OnHealthChange;
    public event Action OnDeath;

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        OnHealthChange?.Invoke(Health);

        if (Health <= 0)
        {
            Death();
        }
        Debug.Log(Health);
    }

    protected virtual void Death()
    {
        OnDeath?.Invoke();
        //Destroy(gameObject);
    }
}
