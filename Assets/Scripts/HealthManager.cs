using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager
{
    private int health;
    private int maxHealth;

    public event EventHandler OnHealthChanged;

    public HealthManager(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int value)
    {
        health -= value;
        if (health < 0)
        {
            health = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
