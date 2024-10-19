using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WeaponStateMachine : StateMachine
{
    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _move;
    [SerializeField] private KeyCode _shootCode;
    [SerializeField] private int _mouseClickNumber;
    [SerializeField] private bool _mouseClickInput = false;
    [SerializeField] private WeaponShootState _shoot;

    [Space(15)] [SerializeField] private PlayerMovement _playerMovement;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnEnable()
    {
        base.OnEnable();
        if (_mouseClickInput)
        {
            switch (_mouseClickNumber)
            {
                case 0:
                    _shootCode = KeyCode.Mouse0;
                    break;
                case 1:
                    _shootCode = KeyCode.Mouse1;
                    break;
            }
        }

        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Input.GetKeyDown(_shootCode) && _shoot.CanShoot)
            {
                ChangeState(_shoot);
            }

            if (Input.GetKey(_shootCode) && _shoot.CanShoot)
            {
                ChangeState(_shoot);
            }

            if (_playerMovement.Moving)
            {
                ChangeState(_move);
                Debug.Log("Moving");
                return;
            }

            ChangeState(_idle);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}