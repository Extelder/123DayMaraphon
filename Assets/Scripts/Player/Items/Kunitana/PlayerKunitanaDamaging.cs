using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerKunitanaDamaging : MonoBehaviour
{
    [Inject] private PlayerHealth _health;
    [SerializeField] private Animator[] _weaponAnimators;
    [SerializeField] private KunitanaHarakiriState _kunitanaHarakiriState;
    [SerializeField] private float _damage;
    [SerializeField] private float _animatorSpeedMultipier = 1.5f;
    [SerializeField] private float _boostRecovery;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _kunitanaHarakiriState.HarakiriPerformed += AddDamage;
    }

    private void OnDisable()
    {
        _kunitanaHarakiriState.HarakiriPerformed -= AddDamage;
    }

    public void AddDamage()
    {
        _health.TakeDamage(_damage);
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        for (int i = 0; i < _weaponAnimators.Length; i++)
        {
            _weaponAnimators[i].speed *= _animatorSpeedMultipier;
        }

        _coroutine = StartCoroutine(DisableBoost());
    }

    private IEnumerator DisableBoost()
    {
        yield return new WaitForSeconds(_boostRecovery);
        for (int i = 0; i < _weaponAnimators.Length; i++)
        {
            _weaponAnimators[i].speed = 1f;
        }
    }
}