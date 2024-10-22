using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDash : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDownForce;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashDownCooldown;
    [Space(10)] [SerializeField] private Transform _orientation;
    [SerializeField] private float _targetMoveSpeed;
    [SerializeField] private float _speedChangeFactor;

    [Inject] private PlayerInputs _playerInputs;

    private Rigidbody _rigidbody;

    private bool _cooldownRecovered = true;

    private CompositeDisposable _dashDisposable = new CompositeDisposable();
    private CompositeDisposable _dashDownDisposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void AddImpulse(Vector3 forceToApply, float cooldown, CompositeDisposable disposable)
    {
        StopAllCoroutines();

        if (!_cooldownRecovered)
            return;

        StartCoroutine(SmoothlyLerpMoveSpeed(forceToApply));
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        _cooldownRecovered = false;

        CoolDown.Timer(cooldown, () => { _cooldownRecovered = true; }, disposable);
    }

    public void Dash()
    {
        _dashDisposable.Clear();

        Vector3 inputDirection = new Vector3(_playerInputs.MovementHorizontal, 0, _playerInputs.MovementVertical);

        if (inputDirection == Vector3.zero)
            inputDirection = _orientation.forward;

        Vector3 direction = _orientation.TransformDirection(inputDirection);

        Vector3 forceToApply = (direction * _dashSpeed + _orientation.up * _dashUpwardForce);
        float cooldown = _dashCooldown;

        AddImpulse(forceToApply, cooldown, _dashDisposable);
    }

    public void DashDown()
    {
        _dashDownDisposable.Clear();

        Vector3 forceToApply = (_orientation.up * _dashDownForce);
        float cooldown = _dashDownCooldown;

        AddImpulse(forceToApply, cooldown, _dashDownDisposable);
    }

    private IEnumerator SmoothlyLerpMoveSpeed(Vector3 velocity)
    {
        float time = 0;
        float difference = Mathf.Abs(_targetMoveSpeed - _dashSpeed);
        float startValue = _dashSpeed;
        float boostFactor = _speedChangeFactor;

        while (time < difference)
        {
            _dashSpeed = Mathf.Lerp(startValue, _targetMoveSpeed, time / difference);

            _rigidbody.velocity += velocity;

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        _dashSpeed = startValue;
    }

    private void OnDisable()
    {
        _dashDisposable.Clear();
    }
}