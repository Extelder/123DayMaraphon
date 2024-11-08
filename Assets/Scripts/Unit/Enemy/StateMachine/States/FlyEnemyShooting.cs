using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class FlyEnemyShooting : State
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private FlyEnemyStateMachine _stateMachine;
    [SerializeField] private float _maxRandomToChangeState;
    [SerializeField] private float _range = 500f;

    [Inject] private Pools _pools;

    public override void Enter()
    {
        StopAllCoroutines();

        Vector3 direction = _shootPoint.position + _shootPoint.forward * _range;
        Projectile projectile = _pools.FPVProjectilePool
            .GetFreeElement(_shootPoint.position, Quaternion.FromToRotation(_shootPoint.position, direction))
            .GetComponent<Projectile>();
        projectile.Initiate(direction);

        StartCoroutine(WaitingForChangeState());
    }

    public override void Exit()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        Exit();
    }

    private IEnumerator WaitingForChangeState()
    {
        yield return new WaitForSeconds(_maxRandomToChangeState);
        _stateMachine.DefaultState();
    }
}