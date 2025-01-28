using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public enum WeaponSoundPool
{
    ShotGun,
    RailGun,
    RPG,
    Rifle
}

public class OnWeaponShootSound : MonoBehaviour
{
    [SerializeField] private WeaponSoundPool _weaponSoundPool;
    [SerializeField] private WeaponShoot _weaponShoot;

    [Inject] private Pools _pools;

    private Pool _currentSoundPool;

    private void Awake()
    {
        switch (_weaponSoundPool)
        {
            case WeaponSoundPool.ShotGun:
                _currentSoundPool = _pools.ShotGunPool;
                break;
            case WeaponSoundPool.RailGun:
                _currentSoundPool = _pools.RailgunPool;

                break;
            case WeaponSoundPool.RPG:
                _currentSoundPool = _pools.RPGPool;

                break;
            case WeaponSoundPool.Rifle:
                _currentSoundPool = _pools.RiflePool;

                break;
        }
    }

    private void OnEnable()
    {
        _weaponShoot.ShootPerformed += OnShootPerformed;
    }

    private void OnShootPerformed()
    {
        _currentSoundPool.GetFreeElement(transform.position, quaternion.identity, transform);
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformed -= OnShootPerformed;
    }
}