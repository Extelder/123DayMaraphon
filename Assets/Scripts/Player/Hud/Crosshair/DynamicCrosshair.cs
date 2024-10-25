using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DynamicCrosshair : MonoBehaviour
{
    [SerializeField] private WeaponShoot _weaponShoot;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootAnimationTrigger;

    private void OnEnable()
    {
        _weaponShoot.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformed -= OnShootPerformed;
    }

    private void OnShootPerformed()
    {
        _animator.SetTrigger(_shootAnimationTrigger);
    }
}