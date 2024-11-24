using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MovementSpeedLerping
{
    [Inject] private PlayerInputs _inputs;
    private Rigidbody _rigidbody;
    private BoolReactiveProperty _moving = new BoolReactiveProperty();
    private CompositeDisposable _walkDisposable = new CompositeDisposable();
    private Coroutine _smoothlyLerpMoveSpeedCoroutine;
    private Coroutine _smoothlyLerpMoveSpeedToStartValue;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moving.Subscribe(_ =>
        {
            if (_)
            {
                if (_smoothlyLerpMoveSpeedToStartValue != null)
                {
                    StopCoroutine(_smoothlyLerpMoveSpeedToStartValue);
                }
                _smoothlyLerpMoveSpeedCoroutine =
                    StartCoroutine(SmoothlyLerpMoveSpeed());
                Debug.Log("moving");
                return;
            }

            if (_smoothlyLerpMoveSpeedCoroutine != null)
            {
                StopCoroutine(_smoothlyLerpMoveSpeedCoroutine);
                _smoothlyLerpMoveSpeedToStartValue =
                    StartCoroutine(SmoothlyLerpMoveSpeedToStartValue());
                Debug.Log("notmoving");
            }
        }).AddTo(_walkDisposable);
    }

    public void Walk(Vector3 input)
    {
        _moving.Value =
            (_inputs.PlayerMovementInputs.MovementHorizontal != 0 ||
             _inputs.PlayerMovementInputs.MovementVertical != 0);
        _rigidbody.velocity = transform.rotation * new Vector3(input.x * moveSpeed,
            _rigidbody.velocity.y, input.z * moveSpeed);
    }

    private void OnDisable()
    {
        _walkDisposable.Clear();
        if (_smoothlyLerpMoveSpeedToStartValue != null)
            StopCoroutine(_smoothlyLerpMoveSpeedToStartValue);
    }
}