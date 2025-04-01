using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MovementSpeedLerping
{
    [SerializeField] private float _acceleration = 16;
    [SerializeField] private float _decceleration = 48;
    [SerializeField] private GroundChecker _groundChecker;

    [Inject] private PlayerInputs _inputs;
    private Rigidbody _rigidbody;
    public BoolReactiveProperty _moving = new BoolReactiveProperty();
    private CompositeDisposable _walkDisposable = new CompositeDisposable();
    private Coroutine _smoothlyLerpMoveSpeedCoroutine;
    private Coroutine _smoothlyLerpMoveSpeedToStartValue;

    [SerializeField] private Vector3 _currentVelocity;

    private bool moving;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // _moving.Subscribe(_ =>
        // {
        //     if (_)
        //     {
        //         if (_smoothlyLerpMoveSpeedToStartValue != null)
        //         {
        //             StopCoroutine(_smoothlyLerpMoveSpeedToStartValue);
        //         }
        //
        //         _smoothlyLerpMoveSpeedCoroutine =
        //             StartCoroutine(SmoothlyLerpMoveSpeed());
        //         return;
        //     }
        //
        //     if (_smoothlyLerpMoveSpeedCoroutine != null)
        //     {
        //         StopCoroutine(_smoothlyLerpMoveSpeedCoroutine);
        //         _smoothlyLerpMoveSpeedToStartValue =
        //             StartCoroutine(SmoothlyLerpMoveSpeedToStartValue());
        //     }
        // }).AddTo(_walkDisposable);
    }

    public void Walk(Vector3 input)
    {
        _moving.Value =
            (_inputs.PlayerMovementInputs.MovementHorizontal != 0 ||
             _inputs.PlayerMovementInputs.MovementVertical != 0);
        input = transform.TransformDirection(input);

        input.Normalize();

        Vector3 desiredVelocityXZ = new Vector3(input.x * moveSpeed, 0, input.z * moveSpeed);

        if (_moving.Value == true || _groundChecker.Detected == true)
            _currentVelocity = Vector3.MoveTowards(_currentVelocity, desiredVelocityXZ, _acceleration * Time.deltaTime);
        else if (_moving.Value == false)
        {
            _currentVelocity =
                Vector3.MoveTowards(_currentVelocity, desiredVelocityXZ, _decceleration * Time.deltaTime);
        }

        _rigidbody.velocity =
            new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _currentVelocity.z);

        //Rigidbody.velocity = transform.rotation * new Vector3(input.x * moveSpeed, Rigidbody.velocity.y, input.z * moveSpeed);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _currentVelocity = velocity;
    }

    private void OnDisable()
    {
        _walkDisposable.Clear();
        if (_smoothlyLerpMoveSpeedToStartValue != null)
            StopCoroutine(_smoothlyLerpMoveSpeedToStartValue);
    }
}