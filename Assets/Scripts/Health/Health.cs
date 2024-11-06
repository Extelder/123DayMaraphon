using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [field: SerializeField] public float MaxValue { get; private set; }
    protected float CurrentValue { get; private set; }
    
    public event Action<float> HealthValueChanged;
    public event Action<float> OnHealedToMax;
    public event Action<float> Damaged;

    private void Awake()
    {
        CurrentValue = MaxValue;
    }

    public void TakeDamage(float value)
    {
        Damaged?.Invoke(CurrentValue);
        if (CurrentValue - value > 0)
        {
            ChangeHealthValue(CurrentValue - value);
            return;
        }

        Death();
    }

    public void Heal(float value)
    {
        if (CurrentValue + value < MaxValue)
        {
            ChangeHealthValue(CurrentValue + value);
            return;
        }

        HealToMax();
    }

    public bool IsDead() => CurrentValue <= 0;
    
    public void HealToMax()
    {
        ChangeHealthValue(MaxValue);
        OnHealedToMax?.Invoke(MaxValue);
    }

    public void Armor(float addibleHealth)
    {
        MaxValue *= addibleHealth;
        CurrentValue = MaxValue;
    }

    public abstract void Death();

    public virtual void ChangeHealthValue(float value)
    {
        if (CurrentValue > 0)
        {
            CurrentValue = value;
            HealthValueChanged?.Invoke(CurrentValue);
        }
    }
}