using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnemyHealthByDisableAndEnableObjects))]
public class SpawnHealsAfterEnemyDisable : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    [Inject] private Pools _pools;

    private EnemyHealthByDisableAndEnableObjects _health;

    private void OnEnable()
    {
        _health = GetComponent<EnemyHealthByDisableAndEnableObjects>();
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
