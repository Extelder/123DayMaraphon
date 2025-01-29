using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWeaponShootTimeStop : MonoBehaviour
{
    [SerializeField] private float _stoppedTime;
    [SerializeField] private WeaponShoot _weaponShoot;

    private void OnEnable()
    {
        _weaponShoot.ShootPerformed += OnShootPerformed;
    }

    private void OnShootPerformed()
    {
        PlayerTime.Instance.TimeStop(_stoppedTime);
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformed -= OnShootPerformed;
    }
}