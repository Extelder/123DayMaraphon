using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerDashDown : MonoBehaviour
{

    [Header("Settings")][SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDownCooldown;
    [Space(10)][SerializeField] private Transform _orientation;
    [SerializeField] private float _targetMoveSpeed;
    [SerializeField] private float _speedChangeFactor;

    private Rigidbody _rigidbody;

    private bool _cooldownRecovered = true;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void DashDown()
    {
        if (!_cooldownRecovered)
            return;

        StartCoroutine(SmoothlyLerpMoveSpeed());
        Vector3 forceToApply = (_orientation.up * -_dashUpwardForce); 
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        _cooldownRecovered = false;

        CoolDown.Timer(_dashDownCooldown, () => { _cooldownRecovered = true; }, _disposable);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(_targetMoveSpeed - _dashSpeed);
        float startValue = _dashSpeed;
        float boostFactor = _speedChangeFactor;

        while (time < difference)
        {
            _dashSpeed = Mathf.Lerp(startValue, _targetMoveSpeed, time / difference);

            _rigidbody.velocity += (_orientation.up * -_dashUpwardForce);

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
