using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathHypeHandler : MonoBehaviour
{
    public IHypeMeasurable Measurable { get; private set; }

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
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }
}