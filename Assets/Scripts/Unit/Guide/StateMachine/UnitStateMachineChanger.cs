using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class UnitStateMachineChanger : MonoBehaviour
{
    [Inject] private PlayerCharacter _character;
    [SerializeField] private GuideRobotStateMachine _stateMachine;
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    private void OnEnable()
    {
        _character.PlayerWalk.Moving.Subscribe(moving =>
        {
            OnPlayerMoving(moving);
        }).AddTo(_disposable);
    }

    private void OnPlayerMoving(bool moving)
    {
        if (moving)
        {
            _stateMachine.Move();
            return;
        }
        _stateMachine.Idle();
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
