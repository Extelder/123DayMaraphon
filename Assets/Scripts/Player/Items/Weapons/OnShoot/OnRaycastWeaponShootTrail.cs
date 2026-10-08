using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OnRaycastWeaponShootTrail : MonoBehaviour
{
    [SerializeField] private RaycastWeaponShoot _weaponShoot;
    [SerializeField] private Transform _nullHitSafeTrailTargetPoint;
    [SerializeField] private Transform _spawnPoint;
    [Inject] public Pools Pool { get; private set; }

    protected Pool pool;

    private Material _material;

    protected RaycastWeaponShoot WeaponShoot => _weaponShoot;

    private void OnEnable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit += ShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit -= ShootPerformed;
    }

    private void Awake()
    {
        SetPool();
    }

    public virtual void SetPool()
    {
        pool = Pool.TrailPool;
    }

    protected virtual ShotTracer.Style TracerStyle =>
        _weaponShoot.Kind == RaycastShotKind.Buckshot ? ShotTracer.Style.Pellet : ShotTracer.Style.Bullet;

    private void ShootPerformed(RaycastHit? hit)
    {
        Vector3 point = hit.HasValue
            ? hit.Value.point
            : _nullHitSafeTrailTargetPoint.position + _weaponShoot.CurrentShootOffset;

        // Материал берём у трейла из пула — стиль остаётся тем же, что был у старых трейлов.
        if (_material == null)
        {
            TrailRenderer template = pool.GetComponentInChildren<TrailRenderer>(true);
            if (template != null)
                _material = template.sharedMaterial;
        }

        ShotTracer.Fire(_spawnPoint, point, TracerStyle, _material, hit.HasValue);
    }
}
