using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private Pool _impactsPool;

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
        var currentObject = _impactsPool.GetFreeElement(hit.point, Quaternion.Euler(0f, 0f, 0f));
    }
}
