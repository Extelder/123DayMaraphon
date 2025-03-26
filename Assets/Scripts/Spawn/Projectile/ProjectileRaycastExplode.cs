using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRaycastExplode : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Projectile _projectile;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        _projectile.HitExplode();
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        _projectile.HitExplode();
    }

    public void Visit(Projectile projectile)
    {
    }

    public void Visit(Ghost ghost, float damage)
    {
    }
}
