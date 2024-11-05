using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class PassiveEnemyState : State
{
    private List<IBuffable> _buffables = new List<IBuffable>();
    private CompositeDisposable _disposable = new CompositeDisposable();

    [SerializeField] private Collider _collider;

    public override void Enter()
    {
        _collider.OnTriggerEnterAsObservable().Subscribe(_ =>
        {
            if (_.TryGetComponent<IBuffable>(out IBuffable _buffable))
            {
                Buff(_buffable);
            }
        }).AddTo(_disposable);

        _collider.OnTriggerExitAsObservable().Subscribe(_ =>
        {
            if (_.TryGetComponent<IBuffable>(out IBuffable _buffable))
            {
                DeBuff(_buffable);
            }
        }).AddTo(_disposable);
    }

    private void Buff(IBuffable _buffable)
    {
        _buffable.Buff();
        _buffables.Add(_buffable);
    }

    private void DeBuff(IBuffable _buffable)
    {
        _buffable.DeBuff();
        _buffables.Remove(_buffable);
    }

    public override void Exit()
    {
        foreach (var buffable in _buffables)
        {
            if (buffable == null)
                continue;
            DeBuff(buffable);
        }

        _buffables.Clear();
        _disposable.Clear();
    }

    private void OnDisable()
    {
        Exit();
    }
}