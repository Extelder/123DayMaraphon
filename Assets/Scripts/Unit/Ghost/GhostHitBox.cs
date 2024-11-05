using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Ghost _ghost;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        DamageTrapedUnits(raycastWeaponShoot.Weapon.DamagePerHit);
    }

    public void Visit(Projectile projectile)
    {
        DamageTrapedUnits(projectile.Damage);
    }

    public void Visit(Ghost ghost, float damage)
    {
    }

    private void DamageTrapedUnits(float damage)
    {
        foreach (var traped in _ghost.TrapedUnits)
        {
            if (traped == null)
                return;
            traped.ObjectVisitor.Visit(_ghost, damage);
        }
    }
}