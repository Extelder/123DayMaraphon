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

    private void OnEnable()
    {
        _weaponShoot.ShootPerformed += OnShootPerformed;
        UnitHitBox.UnitHitted += OnUnitHitted;
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
    }

    private void OnShootPerformed()
    {
        _animator.SetTrigger(_shootAnimationTrigger);
    }
}