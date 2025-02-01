using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyChooseRandomPointAndMove : EnemyState
{
    [SerializeField] private float _walkRadius;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;

    private NavMeshHit _hit;
    

    public override void Enter()
    {
        Animator.Move();
        StartCoroutine(ChooseRandomPoint());
    }
    
    public override void Exit()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChooseRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _walkRadius;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out _hit, _walkRadius, 1);
        Vector3 finalPosition = _hit.position;
        _agent.destination = finalPosition;
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (_agent.remainingDistance <= 1)
            {
                _enemyStateMachine.Attack();    
            }
        }
    }
}