using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyNavMeshChasePlayerState : EnemyState
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _updateTargetMovePositionRate = 0.2f;

    [Inject] private PlayerCharacter _character;

    private Transform _target;

    private void Start()
    {
        _target = _character.Transform;
    }

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