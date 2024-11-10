using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyRandomNavMeshAboutPlayerState : EnemyState
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [Inject] private PlayerCharacter _character;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void Enter()
    {
        Animator.Move();

        _navMeshAgent.SetDestination(_character.RandomNavmeshLocation());
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_navMeshAgent.remainingDistance <= 1)
                _navMeshAgent.SetDestination(_character.RandomNavmeshLocation());
        }).AddTo(_disposable);
    }

    public override void Exit()
    {
        _disposable.Clear();
    }

    private void OnDisable()
    {
        Exit();
    }
}