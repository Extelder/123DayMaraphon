using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Health))]
public class SpawnHealsAfterEnemyDeath : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    [Inject] private Pools _pools;

    private Health _health;

    private void OnEnable()
    {
        _health = GetComponent<Health>();
        _health.Dead += OnDead;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }

    private void OnDead()
    {
        _pools.HealPickupPool.GetFreeElement(_spawnPoint.position);
        Destroy(this);
    }
}