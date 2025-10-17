using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMultipliers : MonoBehaviour
{
    [SerializeField] private float _defaultDamageMultiply = 1;
    [SerializeField] private float _defaultHPMultiply = 1;

    [SerializeField] private float _damagePercent = .12f;
    [SerializeField] private float _hpPercent = .25f;

    [SerializeField] private Wave _wave;

    public static float CurrentDamageMultilier { get; private set; }
    public static float CurrentHPMultilier { get; private set; }

    private void OnEnable()
    {
        _wave.PreStarted += OnStarted;
    }

    private void Start()
    {
        CurrentDamageMultilier = _defaultDamageMultiply;
        CurrentHPMultilier = _defaultHPMultiply;
    }

    private void OnStarted(int value)
    {
        CurrentDamageMultilier = 1 + _damagePercent * (value - 1);
        CurrentHPMultilier = 1 + _hpPercent * (value - 1);
    }

    private void OnDisable()
    {
        _wave.PreStarted -= OnStarted;
    }
}