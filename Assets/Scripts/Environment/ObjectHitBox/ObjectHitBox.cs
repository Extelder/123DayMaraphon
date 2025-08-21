using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectHitBox : MonoBehaviour, IWeaponVisitor
{
    [Inject] private Pools _pools;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        DefaultRaycastVisit(hit);
    }

    public void Visit(Projectile projectile)
    {
        
    }

    public void DefaultRaycastVisit(RaycastHit raycastHit)
    {
        SpawningDecal(raycastHit);
    }

    private void SpawningDecal(RaycastHit hit)
    {
        var currentObject = _pools.DefaultImpactPool.GetFreeElement(hit.point, Quaternion.identity);
    }

    public void Visit(Ghost ghost, float damage)
    {
    }

    public void Visit(PlayerSlashProjectile slashProjectile)
    {
    }
}