using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

public class PlayerDashFOV : MonoBehaviour
{
    [SerializeField] private PlayerDash _dash;
    [SerializeField] private float _dashFOVMultiplyier;
    [SerializeField] private float _lerpDashSpeed;
    [SerializeField] private float _lerpDefaultSpeed;
    [SerializeField] private CinemachineVirtualCamera _cinemachine;
    [SerializeField] private PlayerFOV _playerFOV;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private float _defaultFOV;
    private float _targetFOV;

    private void OnEnable()
    {
        _playerFOV.OnFOVChanged += OnFOVChanged;
        _dash.Dashed += OnDashed;
    }

    private void OnFOVChanged(float value)
    {
        _defaultFOV = value;
        _disposable.Clear();
    }

    private void OnDashed()
    {
        float calcululateDashFOV = _defaultFOV * _dashFOVMultiplyier;
        _targetFOV = calcululateDashFOV;
        float time = _lerpDashSpeed;

        _disposable.Clear();

        Observable.EveryUpdate().Subscribe(_ =>
        {
            _cinemachine.m_Lens.FieldOfView =
                Mathf.Lerp(_cinemachine.m_Lens.FieldOfView, _targetFOV, time * Time.deltaTime);

            if (_targetFOV == calcululateDashFOV && calcululateDashFOV - _cinemachine.m_Lens.FieldOfView < 1)
            {
                _targetFOV = _defaultFOV;
                time = _lerpDefaultSpeed;
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _playerFOV.OnFOVChanged -= OnFOVChanged;
        _dash.Dashed += OnDashed;
        _disposable.Clear();
    }
}