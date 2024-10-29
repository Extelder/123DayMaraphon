using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWeaponShootAddImpulse : MonoBehaviour
{
    [SerializeField] private WeaponShoot _weapon;
    [SerializeField] private Transform _transformInTheOppositeDirectionFromWhereTheImpulseWillComeFrom;
    [SerializeField] private float _impulse;
    [SerializeField] private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _weapon.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weapon.ShootPerformed -= OnShootPerformed;
    }

    private void OnShootPerformed()
    {
        _rigidbody.AddForce(-_transformInTheOppositeDirectionFromWhereTheImpulseWillComeFrom.forward * _impulse, ForceMode.Impulse);
    }
}