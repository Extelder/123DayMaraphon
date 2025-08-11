using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

public class GhostHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private WeaponItem _railGunWeaponItem;

    [SerializeField] private Animator _animator;
    [SerializeField] private string _rpgShootedTriggetName = "RpgShoot";
    [SerializeField] private string _lightningShootedTriggetName = "LightningShoot";

    [SerializeField] private Ghost _ghost;
    [SerializeField] private Pools _pools;

    public event Action RailGunHitted;
    public event Action RPGProjectilHitted;

    private Vector3 _defaultScale;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }

    private void OnDisable()
    {
        _animator.ResetTrigger(_rpgShootedTriggetName);
        _animator.ResetTrigger(_lightningShootedTriggetName);
        transform.localScale = _defaultScale;
    }


    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        DefaultHit(kunitanShoot.Damage, transform.position);
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        DefaultHit(kunitanShoot.Damage, transform.position);
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        DefaultHit(raycastWeaponShoot.Weapon.DamagePerHit, hit.point);

        if (raycastWeaponShoot.Weapon == _railGunWeaponItem)
        {
            RailGunHitted?.Invoke();
            _ghost.GhostRadiusMultiplier = 2;
            _animator.SetTrigger(_rpgShootedTriggetName);
        }
    }

    public void Visit(Projectile projectile)
    {
        if (projectile as LightningBall)
        {
            Debug.LogError("Ghost");
            _ghost.GhostRadiusMultiplier = 3;
            _animator.SetTrigger(_lightningShootedTriggetName);
        }

        RPGProjectilHitted?.Invoke();
        DamageTrapedUnits(projectile.Damage * 2);
        DefaultHit(projectile.Damage, transform.position);
    }


    public void Visit(Ghost ghost, float damage)
    {
    }

    private void DefaultHit(float damage, Vector3 vfxPosition)
    {
        DamageTrapedUnits(damage);
        _pools.GhostBloodExplodePool.GetFreeElement(vfxPosition);
    }

    private void DamageTrapedUnits(float damage)
    {
        foreach (var traped in _ghost.TrapedUnits.ToList())
        {
            if (traped.ObjectVisitor != null)
            {
                traped.ObjectVisitor.Visit(_ghost, damage);
                Debug.Log(traped.ObjectVisitor);
            }
        }
    }
}