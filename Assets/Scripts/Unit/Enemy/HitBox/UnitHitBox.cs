using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitHitBox : RaycastBehaviour, IWeaponVisitor
{
    [SerializeField] private Health _health;
    [Inject] private Pools _pools;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        _health.TakeDamage(raycastWeaponShoot.Weapon.DamagePerHit);
        SpawningDecal(hit.point);
    }

    public void Visit(Projectile projectile)
    {
        if (!_health)
            return;
        if (_health.IsDead())
            return;
        _health.TakeDamage(projectile.Damage);
        SpawningDecal(transform.position);
    }

    public void Visit(Ghost ghost, float damage)
    {
        SpawningDecal(transform.position);
        _health.TakeDamage(damage);
    }

    private void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _pools.BloodExplodePool.GetFreeElement(spawnPoint, Quaternion.identity);
    }
}