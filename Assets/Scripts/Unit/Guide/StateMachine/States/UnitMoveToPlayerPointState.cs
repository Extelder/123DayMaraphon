using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class UnitMoveToPlayerPointState : EnemyState
{
    [Inject] private PlayerCharacter _player;
    [SerializeField] private Transform _origin;
    [SerializeField] private float _maxDistanceDelta;
    [SerializeField] private Collider _collider;
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    public override void Enter()
    {
        _disposable.Clear();
        Animator.Move();
        _collider.enabled = false;
        Move();
    }

    private void Move()
    {
        Observable.Interval(TimeSpan.FromSeconds(0.02)).Subscribe(_ =>
        {
            _origin.position = Vector3.MoveTowards(_origin.position ,_player.GuideTargetPoint.position, _maxDistanceDelta);
            if (Vector3.Distance(_origin.position, _player.GuideTargetPoint.position) <= 0.1)
            {
                _origin.SetParent(_player.GuideTargetPoint);
                _disposable.Clear();
            }
        }).AddTo(_disposable);
    }

    public override void Exit()
    {
        base.Exit();
        _collider.enabled = true;
        _origin.SetParent(null);
        _disposable.Clear();
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
