using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileWeaponShoot : WeaponShoot
{
    [SerializeField] private Transform _muzzle;

    [Inject] private Pools _pools;

    private Pool _currentPool;

    private void Awake()
    {
        Initiate();
    }

    public virtual void Initiate()
    {
        _currentPool = _pools.DefaultProjectilePool;
    }

    public override void OnShootPerformed()
    {
        base.OnShootPerformed();

        CameraShakeInvoke();
        Vector3 direction = Camera.position + Camera.forward * Range;
        Projectile projectile = _currentPool
            .GetFreeElement(_muzzle.position, Quaternion.FromToRotation(_muzzle.position, direction))
            .GetComponent<Projectile>();
        projectile.Initiate(direction);
    }
}