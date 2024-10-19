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

    private Rigidbody _rigidbody;

    private bool _cooldownRecovered = true;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Dash()
    {
        if (!_cooldownRecovered)
            return;

        Vector3 forceToApply = (_orientation.forward * _dashSpeed + _orientation.up * _dashUpwardForce);
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        _cooldownRecovered = false;

        CoolDown.Timer(_dashCooldown, () => { _cooldownRecovered = true; }, _disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}