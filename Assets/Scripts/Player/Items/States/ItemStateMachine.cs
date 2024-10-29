using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ItemStateMachine : StateMachine
{
    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _move;

    [Space(15)] [SerializeField] private GroundChecker _groundChecker;

    [Inject] private PlayerInputs _inputs;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnEnable()
    {
        base.OnEnable();

        Observable.EveryUpdate().Subscribe(_ =>
        {
            bool movingOnGround =
                (Mathf.Abs(_inputs.MovementHorizontal) > 0 || Mathf.Abs(_inputs.MovementVertical) > 0) &&
                _groundChecker.Detected;

            if (movingOnGround)
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