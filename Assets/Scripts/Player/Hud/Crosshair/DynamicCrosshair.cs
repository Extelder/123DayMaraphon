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
    [SerializeField] private Pool _pool;
    [SerializeField] private Pool _bluePool;

    private void OnEnable()
    {
        ProjectileRaycastExplode.ProjectileShooted += OnProjectileShooted;
        _weaponShoot.ShootPerformed += OnShootPerformed;
        UnitHitBox.UnitHitted += OnUnitHitted;
        LightningBall.Hitted += OnLightningBallHitted;
    }

    private void OnProjectileShooted()
    {
        PoolObject instance = _bluePool.GetFreeElement(Vector3.zero);
        instance.transform.localPosition = Vector3.zero;
    }

    private void OnLightningBallHitted()
    {
        PoolObject instance = _bluePool.GetFreeElement(Vector3.zero);
        instance.transform.localPosition = Vector3.zero;
    }

    private void OnUnitHitted()
    {
        PoolObject instance = _pool.GetFreeElement(Vector3.zero);
        instance.transform.localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformed -= OnShootPerformed;
        UnitHitBox.UnitHitted -= OnUnitHitted;
        LightningBall.Hitted -= OnLightningBallHitted;
        ProjectileRaycastExplode.ProjectileShooted -= OnProjectileShooted;
    }

    private void OnShootPerformed()
    {
        _animator.SetTrigger(_shootAnimationTrigger);
    }
}