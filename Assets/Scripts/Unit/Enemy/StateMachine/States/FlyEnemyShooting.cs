using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FlyEnemyShooting : State
{
    [SerializeField] private Pool _projectilePool;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private FlyEnemyStateMachine _stateMachine;
    [SerializeField] private float _maxRandomToChangeState;

    public override void Enter()
    {
        StopAllCoroutines();
        _projectilePool.GetFreeElement(_shootPoint.position, Quaternion.identity);
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