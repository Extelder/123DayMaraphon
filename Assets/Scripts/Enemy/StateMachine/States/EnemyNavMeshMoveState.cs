using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshMoveState : EnemyState
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _updateTargetMovePositionRate = 0.2f;
    [SerializeField] private Transform _target;

    public override void Enter()
    {
        Animator.Move();
        StartCoroutine(Moving());
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            yield return new WaitForSeconds(_updateTargetMovePositionRate);
            _navMeshAgent.SetDestination(_target.position);
        }
    }
}