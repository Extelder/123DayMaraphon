using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileRaycastExplode : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private GameObject _projectileGFX;
    [SerializeField] private float _scaleFactor;
    
    private float _defaultExplosionRange;
    private Vector3 _defaultEffectScale;


    private void Start()
    {
        _defaultExplosionRange = _projectile.ExplosionRange;
        _defaultEffectScale = _projectileGFX.transform.localScale;
    }

    private void OnDisable()
    {
        _projectile.ExplosionRange = _defaultExplosionRange;
        _projectileGFX.transform.localScale = _defaultEffectScale;
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        _projectile.HitExplode();
        ScaleProjectile();
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        _projectile.HitExplode();
        ScaleProjectile();
    }

    private void ScaleProjectile()
    {
        _projectile.ExplosionRange *= _scaleFactor;
        _projectileGFX.transform.localScale *= _scaleFactor;
    }

    public void Visit(Projectile projectile)
    {
    }

    public void Visit(Ghost ghost, float damage)
    {
    }
}
