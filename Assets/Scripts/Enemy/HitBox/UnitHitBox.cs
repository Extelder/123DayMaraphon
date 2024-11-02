using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Health _health;
    [Inject] private Pools _pools;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        _health.TakeDamage(raycastWeaponShoot.Weapon.DamagePerHit);
        SpawningDecal(hit.point);
    }

    public void Visit(Projectile projectile)
    {
        _health.TakeDamage(projectile.Damage);
        SpawningDecal(transform.position);
    }

    private void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _pools.BloodExplodePool.GetFreeElement(spawnPoint, Quaternion.identity);
    }
}