using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class FlyEnemyShooting : State
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private FlyEnemyStateMachine _stateMachine;
    [SerializeField] private float _maxRandomToChangeState;
    [SerializeField] private float _minRandomToChangeState;
    [SerializeField] private float _range = 500f;

    [Inject] private Pools _pools;

    public event Action Attacked;

    public override void Enter()
    {
        StopAllCoroutines();

        Attacked?.Invoke();
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
        yield return new WaitForSeconds(Random.Range(_minRandomToChangeState, _maxRandomToChangeState));
        _stateMachine.DefaultState();
    }
}