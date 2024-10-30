using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerStashedShake : MonoBehaviour
{
    [SerializeField] private PlayerDashDown _dashDown;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void OnEnable()
    {
        _dashDown.Stashed += OnStashed;
    }

    private void OnDisable()
    {
        _dashDown.Stashed -= OnStashed;
    }

    private void OnStashed()
    {
        _impulseSource.GenerateImpulse();
    }
}