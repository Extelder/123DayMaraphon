using System;
using System.Collections;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDash : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashCooldown;
    [Space(10)] [SerializeField] private Transform _orientation;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _targetMoveSpeed;
    [SerializeField] private float _speedChangeFactor;

    private Rigidbody _rigidbody;

    private bool _cooldownRecovered = true;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Dash()
    {
        Debug.Log(_playerMovement.CurrentMoveSpeed);
        if (!_cooldownRecovered)
            return;

        StartCoroutine(SmoothlyLerpMoveSpeed());
        Vector3 forceToApply = (_orientation.forward * _playerMovement.CurrentMoveSpeed + _orientation.up * _dashUpwardForce);
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        _cooldownRecovered = false;

        CoolDown.Timer(_dashCooldown, () => { _cooldownRecovered = true; }, _disposable);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(_targetMoveSpeed - _playerMovement.CurrentMoveSpeed);
        float startValue = _playerMovement.CurrentMoveSpeed;
        float boostFactor = _speedChangeFactor;

        while (time < difference)
        {
            _playerMovement.CurrentMoveSpeed = Mathf.Lerp(startValue, _targetMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        _playerMovement.CurrentMoveSpeed = _targetMoveSpeed;
        _speedChangeFactor = 1f;
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}