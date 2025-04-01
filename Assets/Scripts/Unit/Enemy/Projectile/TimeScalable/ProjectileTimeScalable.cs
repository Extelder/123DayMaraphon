using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTimeScalable : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        KunitanaUltimate.Ultimated += OnProjectileTimeScaled;
        KunitanaUltimate.UltimateStoped += OnProjectileUnTimeScaled;
        _projectile.Initiated += OnProjectileInitiated;
        Invoke(nameof(OnProjectileInitiated), 0.01f);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnProjectileInitiated()
    {
        if (KunitanaUltimate.Instance.Ultimating)
        {
            OnProjectileTimeScaled();
        }   
    }

    private void OnProjectileTimeScaled()
    {
        _rigidbody.velocity *= 0.1f;
    }

    private void OnProjectileUnTimeScaled()
    {
        _rigidbody.velocity /= 0.1f;
    }

    private void OnDisable()
    {
        KunitanaUltimate.Ultimated -= OnProjectileTimeScaled;
        KunitanaUltimate.UltimateStoped -= OnProjectileUnTimeScaled;
        _projectile.Initiated -= OnProjectileInitiated;
    }
}
