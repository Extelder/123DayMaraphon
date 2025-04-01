using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

public class GhostHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Ghost _ghost;
    [SerializeField] private Pools _pools;

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
    }

    public void Visit(Projectile projectile)
    {
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