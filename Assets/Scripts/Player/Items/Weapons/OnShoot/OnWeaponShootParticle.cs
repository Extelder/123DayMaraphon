using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWeaponShootParticle : MonoBehaviour
{
    [SerializeField] private WeaponShoot _weaponShoot;
    [SerializeField] private ParticleSystem _particleSystem;

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
        _particleSystem.Play();
    }
}