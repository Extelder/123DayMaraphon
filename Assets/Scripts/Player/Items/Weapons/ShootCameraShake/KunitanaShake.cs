using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class KunitanaShake : MonoBehaviour
{
    [SerializeField] private KunitanShoot _kunitanShoot;
    [SerializeField] private CinemachineImpulseSource _impulse;

    private void OnEnable()
    {
        _kunitanShoot.Shooted += OnShooted;
    }

    private void OnShooted()
    {
        _impulse.GenerateImpulse();
    }
    
    
    private void OnDisable()
    {
        _kunitanShoot.Shooted -= OnShooted;
    }
}
