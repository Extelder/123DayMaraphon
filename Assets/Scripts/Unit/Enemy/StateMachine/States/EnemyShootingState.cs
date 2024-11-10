using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyShootingState : EnemyState
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _range = 500f;

    [Inject] private Pools _pools;

    public override void Enter()
    {
        _agent.isStopped = true;
        CanChanged = false;
        Animator.Attack();
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    public void PerformShoot()
    {
        CanChanged = true;

        Vector3 direction = _shootPoint.position + _shootPoint.forward * _range;
        Projectile projectile = _pools.FPVProjectilePool
            .GetFreeElement(_shootPoint.position, Quaternion.FromToRotation(_shootPoint.position, direction))
            .GetComponent<Projectile>();
        projectile.Initiate(direction);

        Invoke(nameof(AnimationEnd), Random.Range(0, 1));
    }

    public void AnimationEnd()
    {
        _enemyStateMachine.Move();
    }
}