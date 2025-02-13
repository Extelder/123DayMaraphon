using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LerpFovByClickUnClick : MonoBehaviour
{
    [SerializeField] private float _fovDifference;
    [SerializeField] private float _duration;
    [SerializeField] private Camera _camera;

    private float _targetFOV;

    private Tween _tween;

    private void Start()
    {
        _targetFOV = _camera.fieldOfView;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _tween.Kill();
            _targetFOV += _fovDifference;
            _tween = _camera.DOFieldOfView(_targetFOV, _duration);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _tween.Kill();
            _targetFOV -= _fovDifference;
            _tween = _camera.DOFieldOfView(_targetFOV, _duration);
        }
    }

    private void OnDisable()
    {
        _tween.Kill();
    }
}