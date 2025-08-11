using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Projectile, IWeaponVisitor
{
    [SerializeField] private GameObject _defaultExplosion;
    [SerializeField] private GameObject _hitExplosion;

    [SerializeField] private GameObject _explosion;

    [SerializeField] private WeaponItem _railgunWeaponItem;

    [SerializeField] private float _rpgRangeMultipier;
    [SerializeField] private float _railGunRangeMultipier;
    [SerializeField] private float _rpgDamageMultipier;
    [SerializeField] private float _railGunDamageMultipier;

    private float _defaultRange;
    private Vector3 _defaultScale;

    public static event Action Hitted;

    public override void OnDisableVirtual()
    {
        ExplosionRange = _defaultRange;
        _hitExplosion.transform.localScale = _defaultScale;
        _defaultExplosion.SetActive(true);
        _hitExplosion.SetActive(false);
    }

    private void Start()
    {
        _defaultRange = ExplosionRange;
        _defaultScale = _hitExplosion.transform.localScale;
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);

        _hitExplosion.transform.localScale *= _railGunRangeMultipier;
        ExplosionRange *= _railGunRangeMultipier;
        Hitted?.Invoke();
        Explode(_railGunDamageMultipier);
        PlayerTime.Instance.TimeStop(0.3f);
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);

        _hitExplosion.transform.localScale *= _railGunRangeMultipier;
        ExplosionRange *= _railGunRangeMultipier;
        Hitted?.Invoke();
        Explode(_railGunDamageMultipier);
        PlayerTime.Instance.TimeStop(0.3f);
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        if (raycastWeaponShoot.Weapon == _railgunWeaponItem)
        {
            _defaultExplosion.SetActive(false);
            _hitExplosion.SetActive(true);

            _hitExplosion.transform.localScale *= _railGunRangeMultipier;
            ExplosionRange *= _railGunRangeMultipier;
            Hitted?.Invoke();
            Explode(_railGunDamageMultipier);
            PlayerTime.Instance.TimeStop(0.3f);
        }
    }

    public void Visit(Projectile projectile)
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);
        _hitExplosion.transform.localScale *= _rpgRangeMultipier;
        ExplosionRange *= _rpgRangeMultipier;
        Hitted?.Invoke();
        Explode(_rpgDamageMultipier);
        PlayerTime.Instance.TimeStop(0.3f);
    }

    public void Visit(Ghost ghost, float damage)
    {
    }
}