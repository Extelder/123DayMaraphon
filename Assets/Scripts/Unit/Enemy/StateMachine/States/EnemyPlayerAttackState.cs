using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyPlayerAttackState : EnemyState
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _damage;

    public event Action Attacked;
    
    public override void Enter()
    {
        Animator.Attack();
        _agent.isStopped = true;
    }

    public override void Exit()
    {
        _agent.isStopped = false;
    }

    public void AnimationEnd()
    {
        CanChanged = true;
    }

    public void Attack()
    {
        _playerHealth.TakeDamage(_damage);
        Attacked?.Invoke();
    }
}