using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class UnitHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private DeathHypeHandler _deathHypeHandler;
    [SerializeField] private Health _health;
    [Inject] private Pools _pools;

    public IHypeMeasurable CurrentHypeMeasurable { get; private set; }

    public event Action Hit;

    public static event Action UnitHitted;

    public void OnDrawGizmos()
    {
        _deathHypeHandler = GetComponentInParent<DeathHypeHandler>();
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        CurrentHypeMeasurable = kunitanShoot;
        TakeDamage(kunitanShoot.Damage, CurrentHypeMeasurable.HypeType);
        SpawningDecal(transform.position);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public void Visit(KunitanaUltimateAttack kunitanaUltimateAttack)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        CurrentHypeMeasurable = kunitanaUltimateAttack;
        TakeDamage(kunitanaUltimateAttack.Damage, CurrentHypeMeasurable.HypeType);
        SpawningDecal(transform.position);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public virtual void TakeDamage(float damage, HypeType hypeType, float hypeValueMultiplier = 1)
    {
        Debug.Log(CurrentHypeMeasurable.HypeValue * hypeValueMultiplier);
        _deathHypeHandler.SetHype(CurrentHypeMeasurable.HypeValue * hypeValueMultiplier, hypeType);
        _health.TakeDamage(damage);
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        CurrentHypeMeasurable = raycastWeaponShoot;
        TakeDamage(raycastWeaponShoot.Weapon.DamagePerHit, CurrentHypeMeasurable.HypeType);
        SpawningDecal(hit.point);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public void Visit(Projectile projectile)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        CurrentHypeMeasurable = projectile;
        TakeDamage(projectile.Damage, CurrentHypeMeasurable.HypeType);
        SpawningDecal(transform.position);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public void Visit(Ghost ghost, float damage)
    {
        CurrentHypeMeasurable = ghost;
        SpawningDecal(transform.position);
        TakeDamage(damage, CurrentHypeMeasurable.HypeType);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public void Visit(PlayerSlashProjectile slashProjectile, float damage)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        CurrentHypeMeasurable = slashProjectile;
        TakeDamage(slashProjectile.Damage, CurrentHypeMeasurable.HypeType);
        Hit?.Invoke();
        UnitHitted?.Invoke();
    }

    public virtual void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _pools.BloodExplodePool.GetFreeElement(spawnPoint, Quaternion.identity);
    }
}