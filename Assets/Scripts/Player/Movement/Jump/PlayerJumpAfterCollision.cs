using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerJumpAfterCollision : MonoBehaviour
{
    [SerializeField] private WallChecker _wallChecker;
    [SerializeField] private LayerMask _checkLayerMask;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private PlayerWalk _walk;
    [Inject] private PlayerInputs _playerInputs;
    [SerializeField] private Transform _orientation;

    [Space(10), Header("Velocity Settings")] [SerializeField]
    private float _forceBack;

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
        RaycastHit hit;

        Vector3 direction;
        if (Physics.Raycast(transform.position, transform.right, out hit, _raycastDistance, _checkLayerMask))
        {
            if (hit.collider.TryGetComponent<Wall>(out Wall wall))
            {
                Debug.DrawRay(transform.position, transform.right * _raycastDistance, Color.green, 5f);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow, 5f);
                direction = hit.normal;
                //Debug.DrawRay(transform.position, direction, Color.yellow, 5f);
                // direction = (hit.point - transform.position).normalized;
                // direction.Normalize();
                // direction = new Vector3(direction.x, 0, direction.z);

                Vector3 forceToApply = (direction * _forceBack + _orientation.up * _forceUpward);
                _walk.SetVelocity(forceToApply);

                return;
                // StartCoroutine(SmoothlyLerpForce(forceToApply));
                // _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            }
        }

        if (Physics.Raycast(transform.position, -transform.right, out hit, _raycastDistance, _checkLayerMask))
        {
            if (hit.collider.TryGetComponent<Wall>(out Wall wall))
            {
                Debug.DrawRay(transform.position, -transform.right * _raycastDistance, Color.green, 5f);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow, 5f);
                direction = hit.normal;
                //Debug.DrawRay(transform.position, direction, Color.yellow, 5f);
                // direction = (hit.point - transform.position).normalized;
                // direction.Normalize();
                // direction = new Vector3(direction.x, 0, direction.z);

                Vector3 forceToApply = (direction * _forceBack + _orientation.up * _forceUpward);
                _walk.SetVelocity(forceToApply);

                return;
                // StartCoroutine(SmoothlyLerpForce(forceToApply));
                // _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            }
        }
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastDistance, _checkLayerMask))
        {
            if (hit.collider.TryGetComponent<Wall>(out Wall wall))
            {
                Debug.DrawRay(transform.position, transform.forward * _raycastDistance, Color.green, 5f);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow, 5f);
                direction = hit.normal;
                //Debug.DrawRay(transform.position, direction, Color.yellow, 5f);
                // direction = (hit.point - transform.position).normalized;
                // direction.Normalize();
                // direction = new Vector3(direction.x, 0, direction.z);

                Vector3 forceToApply = (direction * _forceBack + _orientation.up * _forceUpward);
                _walk.SetVelocity(forceToApply);

                return;
                // StartCoroutine(SmoothlyLerpForce(forceToApply));
                // _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            }
        }
        
        if (Physics.Raycast(transform.position, -transform.forward, out hit, _raycastDistance, _checkLayerMask))
        {
            if (hit.collider.TryGetComponent<Wall>(out Wall wall))
            {
                Debug.DrawRay(transform.position, -transform.forward * _raycastDistance, Color.green, 5f);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow, 5f);
                direction = hit.normal;
                //Debug.DrawRay(transform.position, direction, Color.yellow, 5f);
                // direction = (hit.point - transform.position).normalized;
                // direction.Normalize();
                // direction = new Vector3(direction.x, 0, direction.z);

                Vector3 forceToApply = (direction * _forceBack + _orientation.up * _forceUpward);
                _walk.SetVelocity(forceToApply);

                return;
                // StartCoroutine(SmoothlyLerpForce(forceToApply));
                // _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
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


    private void OnEnable()
    {
        _playerInputs.PlayerMovementInputs.JumpPressedDown += OnJumpPressedDown;
    }

    private void OnDisable()
    {
        _playerInputs.PlayerMovementInputs.JumpPressedDown -= OnJumpPressedDown;
    }

    private void OnJumpPressedDown()
    {
        TryJumpAfterCollision();
    }
}