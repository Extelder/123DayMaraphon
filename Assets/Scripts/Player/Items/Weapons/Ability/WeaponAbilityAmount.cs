using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WeaponAbilityAmount : Amount
{
    public ReactiveProperty<bool> Filled { get; private set; } = new ReactiveProperty<bool>();

    private void FixedUpdate()
    {
        if (earn)
            Earn();
    }

    private void Earn()
    {
        current += earnSpeed;
        if (current > capacity)
            current = capacity;

        if (!Filled.Value && current >= capacity)
        {
            Filled.Value = true;
            current = capacity;
            return;
        }
    }

    public void SpendAll()
    {
        Spend(capacity);
    }

    public void Spend(float value)
    {
        if (current - value < capacity)
        {
            Filled.Value = false;
        }

        if (current - value <= 0)
        {
            current = 0;
            Debug.Log("FALSE");
            return;
        }

        current -= value;
    }
}