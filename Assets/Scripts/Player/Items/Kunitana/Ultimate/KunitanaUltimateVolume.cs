using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class KunitanaUltimateVolume : MonoBehaviour
{
    [SerializeField] private Volume _ultimatingVolume;
    [SerializeField] private float _changeVolumeSpeed;
    [SerializeField] private float _ultimatingMultiplier = 10;

    private float _targetWeight = 0;

    private float _changeVolumeMultiplier = 1;
    private float _defaultChangeMultiplier;

    private void Awake()
    {
        _defaultChangeMultiplier = _changeVolumeMultiplier;
    }

    private void OnEnable()
    {
        KunitanaUltimate.Ultimated += OnUltimated;
        KunitanaUltimate.UltimateStoped += OnStopUltimating;
    }

    private void OnDisable()
    {
        KunitanaUltimate.Ultimated -= OnUltimated;
        KunitanaUltimate.UltimateStoped -= OnStopUltimating;
    }

    public void OnStopUltimating()
    {
        _changeVolumeMultiplier = _defaultChangeMultiplier;
        _targetWeight = 0;
    }

    private void Update()
    {
        _ultimatingVolume.weight =
            Mathf.Lerp(_ultimatingVolume.weight, _targetWeight,
                _changeVolumeSpeed * _changeVolumeMultiplier * Time.deltaTime);
    }

    private void OnUltimated()
    {
        _changeVolumeMultiplier = _ultimatingMultiplier;
        _targetWeight = 1;
    }
}