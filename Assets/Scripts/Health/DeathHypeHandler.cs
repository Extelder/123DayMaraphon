using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathHypeHandler : MonoBehaviour
{
    public float HypeValue { get; private set; }

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Dead += OnDead;
    }

    private void OnDead()
    {
        PlayerHypeSystem.Instance.Add(HypeValue);
    }

    public void SetHype(float value)
    {
        HypeValue = value;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }
}