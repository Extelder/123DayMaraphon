using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTrapedState : State
{
    [SerializeField] private EnemyTrapAnimator _animator;
    [SerializeField] private NavMeshAgent _agent;

    public override void Enter()
    {
        CanChanged = false;
        _animator.Trap();
        _agent.isStopped = true;
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    public void OnUnTraped()
    {
        CanChanged = true;
    }
}
