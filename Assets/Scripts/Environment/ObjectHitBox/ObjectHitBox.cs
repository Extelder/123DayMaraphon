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

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        DefaultRaycastVisit(hit);
    }

    public void DefaultRaycastVisit(RaycastHit raycastHit)
    {
        SpawningDecal(raycastHit);
    }

    private void SpawningDecal(RaycastHit hit)
    {
        var currentObject = _pools.DefaultImpactPool.GetFreeElement(hit.point, Quaternion.identity);
    }
}