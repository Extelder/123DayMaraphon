using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHypeSystem : MonoBehaviour
{
    [field: SerializeField] public float MaxValue { get; private set; } = 5f;
    [field: SerializeField] public float MinValue { get; private set; } = 0f;
    [field: SerializeField] public float DecreaseValue { get; private set; } = 0.05f;
    [field: SerializeField] public float DecreaseRate { get; private set; } = 0.05f;
    [field: SerializeField] public float ResetDecreaseDelay { get; private set; } = 2f;

    public float Current { get; private set; }
    public float Multiplyer { get; private set; }

    public static PlayerHypeSystem Instance { get; private set; }
    public event Action<float> HypeChanged;

    public bool CanDecreasing { get; private set; }

    private Coroutine _resettingCoroutine;

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
        StartCoroutine(Decreasing());
    }

    private IEnumerator Decreasing()
    {
        while (true)
        {
            yield return new WaitUntil(() => CanDecreasing);
            Remove(DecreaseValue);
            yield return new WaitForSeconds(DecreaseRate);
        }
    }

    public void Add(float value, bool shouldMultiply = true)
    {
        if (shouldMultiply)
            value *= Multiplyer;
        if (Current + value >= MaxValue)
            Current = MaxValue;
        else
            Current += value;
        CanDecreasing = false;
        if (_resettingCoroutine != null)
            StopCoroutine(_resettingCoroutine);
        _resettingCoroutine = StartCoroutine(ResetDecreasing());
        HypeChanged?.Invoke(Current);
    }

    private IEnumerator ResetDecreasing()
    {
        yield return new WaitForSeconds(ResetDecreaseDelay);
        CanDecreasing = true;
    }

    public void Remove(float value, bool shouldMultiply = false)
    {
        if (shouldMultiply)
            value *= Multiplyer;
        if (Current - value <= MinValue)
            Current = MinValue;
        else
            Current -= value;
        HypeChanged?.Invoke(Current);
    }

    public void Multiply(float value)
    {
        Multiplyer *= value;
    }
}