using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHypeSystem : MonoBehaviour
{
    [field: SerializeField] public float MaxValue { get; private set; } = 5f;
    [field: SerializeField] public float MinValue { get; private set; } = 0f;

    public float Current { get; private set; }
    public float Multiplyer { get; private set; }

    public static PlayerHypeSystem Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There`s one more PlayerHypeSystem");
        Debug.Break();
    }

    private void Start()
    {
        Current = 0;
        Multiplyer = 1;
    }

    public void Add(float value, bool shouldMultiply = true)
    {
        if (shouldMultiply)
            value *= Multiplyer;
        if (Current + value >= MaxValue)
            Current = MaxValue;
        else
            Current += value;
    }

    public void Remove(float value, bool shouldMultiply = false)
    {
        if (shouldMultiply)
            value *= Multiplyer;
        if (Current - value <= MinValue)
            Current = MinValue;
        else
            Current -= value;
    }

    public void Multiply(float value)
    {
        Multiplyer *= value;
    }
}