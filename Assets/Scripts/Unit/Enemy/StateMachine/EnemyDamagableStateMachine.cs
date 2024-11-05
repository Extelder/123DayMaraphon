using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagableStateMachine : EnemyStateMachine
{
    [SerializeField] private State _damagable;
    [SerializeField] private Health _enemyHealth;

    private void Start()
    {
        _enemyHealth.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _enemyHealth.Damaged -= OnDamaged;
    }

    private void OnDamaged(float value)
    {
        TakeDamage();
    }

    public void TakeDamage()
    {
        ChangeState(_damagable);
    }
}