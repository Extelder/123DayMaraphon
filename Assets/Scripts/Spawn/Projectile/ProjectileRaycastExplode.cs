using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileRaycastExplode : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private RPGProjectile _projectile;
    [SerializeField] private GameObject _projectileGFX;
    [SerializeField] private GameObject _hittedExplosionGFX;
    [SerializeField] private GameObject _defaultExplosionGFX;

    private float _defaultExplosionRange;
    private Vector3 _defaultEffectScale;

    private void Awake()
    {
        _defaultExplosionRange = _projectile.ExplosionRange;
        _defaultEffectScale = _projectileGFX.transform.localScale;
    }

    private void OnEnable()
    {
        _hittedExplosionGFX.SetActive(false);
        _defaultExplosionGFX.SetActive(true);
        Debug.LogError(_defaultEffectScale);
        _projectile.ExplosionRange = _defaultExplosionRange;
        _projectileGFX.transform.localScale = _defaultEffectScale;
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        _hittedExplosionGFX.SetActive(true);
        _defaultExplosionGFX.SetActive(false);
        _projectile.SearchNearestEnemy();
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        _hittedExplosionGFX.SetActive(true);
        _defaultExplosionGFX.SetActive(false);
        _projectile.SearchNearestEnemy();
    }

    public void Visit(Projectile projectile)
    {
    }

    public void Visit(Ghost ghost, float damage)
    {
    }
}