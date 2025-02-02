using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyPlayerShootAttackState : EnemyState
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _range = 500f;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private LookAtController _lookAtPlayer;
    [SerializeField] private SmoothlyLookAtPlayer _smoothlyLookAtPlayer;

    [Inject] private Pools _pools;

    private Vector3 _startSpineParentEulerAngles;

    public override void Enter()
    {
        Animator.Attack();
        _agent.isStopped = true;
        _agent.updateRotation = false;
        _smoothlyLookAtPlayer.enabled = true;
    }

    public override void Exit()
    {
        _agent.isStopped = false;
        _agent.updateRotation = true;
        _smoothlyLookAtPlayer.enabled = false;
    }

    public void AnimationEnd()
    {
        CanChanged = true;
        _enemyStateMachine.Move();
    }

    public void Attack()
    {
        Vector3 direction = _shootPoint.position + _shootPoint.forward * _range;
        Projectile projectile = _pools.FPVProjectilePool
            .GetFreeElement(_shootPoint.position, Quaternion.FromToRotation(_shootPoint.position, direction))
            .GetComponent<Projectile>();
        projectile.Initiate(direction);
    }
}