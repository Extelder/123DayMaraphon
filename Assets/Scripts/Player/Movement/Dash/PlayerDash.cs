using System;
using System.Collections;
using UniRx;
using UnityEngine;

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

    private Rigidbody _rigidbody;

    private bool _cooldownRecovered = true;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void AddImpulse(Vector3 forceToApply, float cooldown)
    {
        StopAllCoroutines();

        if (!_cooldownRecovered)
            return;

        StartCoroutine(SmoothlyLerpMoveSpeed(forceToApply));
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        _cooldownRecovered = false;

        CoolDown.Timer(cooldown, () => { _cooldownRecovered = true; }, _disposable);
    }

    public void Dash()
    {
        Vector3 forceToApply = (_orientation.forward * _dashSpeed + _orientation.up * _dashUpwardForce);
        float cooldown = _dashCooldown;
        AddImpulse(forceToApply, cooldown);
    }

    public void DashDown()
    {
        Vector3 forceToApply = (_orientation.up * _dashDownForce);
        float cooldown = _dashDownCooldown;
        AddImpulse(forceToApply, cooldown);
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
        _disposable.Clear();
    }
}
