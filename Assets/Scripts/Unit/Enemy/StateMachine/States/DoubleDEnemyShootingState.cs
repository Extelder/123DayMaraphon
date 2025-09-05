using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class DoubleDEnemyShootingState : State
{
    [SerializeField] private DoubleDEnemyStateMachine _enemyStateMachine;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _minCooldown;
    [SerializeField] private float _maxCooldown;

    [Inject] private Pools _pools;

    public event Action Attacked;

    public override void Enter()
    {
        StopAllCoroutines();
        int angle = 90 * Random.Range(1, 3);
        
        Attacked?.Invoke();
        _pools.DoubleDProjectilePool
            .GetFreeElement(_shootPoint.position,  _shootPoint.rotation * Quaternion.Euler(angle, 90, 0));
        StartCoroutine(WaitToChangeState());
    }

    public override void Exit()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        Exit();
    }

    private IEnumerator WaitToChangeState()
    {
        yield return new WaitForSeconds(Random.Range(_minCooldown, _maxCooldown));
        _enemyStateMachine.DefaultState();
    }
}
