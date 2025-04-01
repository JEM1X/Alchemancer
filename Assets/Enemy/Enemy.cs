using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int Health { get => health; }
    [SerializeField] private int health = 5;

    public event Action<int> OnHealthChange;
    public event Action OnDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;

        OnHealthChange?.Invoke(health);

        if (health > 0) return;

        Death();
    }

    private void Death()
    {
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
