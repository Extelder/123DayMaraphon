using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTakeDamageState : EnemyState
{
    [SerializeField] private NavMeshAgent _agent;

    public override void Enter()
    {
        Animator.TakeDamage();
        CanChanged = false;
        _agent.isStopped = true;
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    public void TakeDamageAnimationEnd()
    {
        CanChanged = true;
    }
}