using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AirGunStateMachine : StateMachine
{
    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _move;
    [SerializeField] private State _shoot;

    [Space(15)] [SerializeField] private PlayerMovement _playerMovement;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Start()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ChangeState(_shoot);
            }

            if (_playerMovement.Moving)
            {
                ChangeState(_move);
                return;
            }

            ChangeState(_idle);
        }).AddTo(_disposable);
    }
}