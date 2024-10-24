using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerJumpAfterCollision : MonoBehaviour
{
    [SerializeField] private OverlapSettings _overlapSettings;
    [Inject] private PlayerInputs _playerInputs;
    [SerializeField] private Transform _orientation;
    [Space(10), Header("Velocity Settings")]
    [SerializeField] private float _forceBack;
    [SerializeField] private float _forceUpward;
    [SerializeField] private float _targetSpeed;
    [SerializeField] private float _speedChangeFactor;
    [SerializeField] private float _invertOrientationDashSpeed;

    private float _defaultInvertOrientionSpeed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _defaultInvertOrientionSpeed = _invertOrientationDashSpeed;
    }

    public void TryJumpAfterCollision()
    {
        OverlapSphere();
        StopAllCoroutines();
        Vector3 inputDirection = new Vector3(_playerInputs.MovementHorizontal, 0, _playerInputs.MovementVertical);
        Vector3 direction = _orientation.TransformDirection(inputDirection); 
 
        for (var i = 0; i < _overlapSettings.Size; i++)
        {
            var target = _overlapSettings.Colliders[i].gameObject;
            if (target.TryGetComponent<Wall>(out Wall wall))
            {
                if (inputDirection == Vector3.zero)
                {
                    direction = (target.transform.position - transform.position).normalized;
                    direction = new Vector3(direction.x, 0, direction.z);
                }
                Vector3 forceToApply = (direction * _forceBack + _orientation.up * _forceUpward);
                StartCoroutine(SmoothlyLerpForce(forceToApply));
                _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator SmoothlyLerpForce(Vector3 velocity)
    {
        float time = 0;
        float difference = Mathf.Abs(_targetSpeed - _invertOrientationDashSpeed);
        float startValue = _invertOrientationDashSpeed;
        float boostFactor = _speedChangeFactor;

        while (time < difference)
        {
            _invertOrientationDashSpeed = Mathf.Lerp(startValue, _targetSpeed, time / difference);

            _rigidbody.velocity += velocity;

            time += Time.deltaTime * boostFactor;

            yield return null;
        }
        _invertOrientationDashSpeed = _defaultInvertOrientionSpeed;
    }

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(_overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }

    private void OnEnable()
    {
        _playerInputs.JumpPressedDown += OnJumpPressedDown;
    }

    private void OnDisable()
    {
        _playerInputs.JumpPressedDown -= OnJumpPressedDown;
    }

    private void OnJumpPressedDown()
    {
        TryJumpAfterCollision();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_overlapSettings._overlapPoint.position + _overlapSettings._positionOffset, _overlapSettings._sphereRadius);
    }
}
