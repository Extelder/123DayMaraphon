using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : Ammo
{
    [SerializeField] private WeaponShootState _weaponShootState;
    [SerializeField] private WeaponShoot _shoot;
    [SerializeField] private int _ammoPerShoot;

    private void OnEnable()
    {
        _shoot.ShootPerformed += OnShootPerformed;
        ValueChanged += OnValueChanged;
    }

    private void OnShootPerformed()
    {
        SpendAmmo(_ammoPerShoot);
    }

    private void OnValueChanged(int value)
    {
        Debug.Log(CurrentAmmo);
        
        if (IsOut())
        {
            _weaponShootState.CanShoot = false;
        }
        else
        {
            _weaponShootState.CanShoot = true;
        }
    }

    private void OnDisable()
    {
        _shoot.ShootPerformed -= OnShootPerformed;
        ValueChanged -= OnValueChanged;
    }
}