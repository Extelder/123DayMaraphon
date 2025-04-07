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
    public event Action Dead;

    private void Start()
    {
        CurrentValue = MaxValue;
    }

    public virtual void TakeDamage(float value)
    {
        if (IsDead())
            return;

        if (CurrentValue - value > 0)
        {
            ChangeHealthValue(CurrentValue - value);
            Damaged?.Invoke(CurrentValue);
            return;
        }

        Damaged?.Invoke(CurrentValue);

        Dead?.Invoke();
        Death();
    }

    public void Heal(float value)
    {
        if (IsDead())
            return;
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
        if (IsDead())
            return;
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