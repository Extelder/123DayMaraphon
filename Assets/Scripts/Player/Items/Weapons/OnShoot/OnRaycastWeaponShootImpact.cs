using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRaycastWeaponShootImpact : MonoBehaviour
{
    [SerializeField] private RaycastWeaponShoot _weaponShoot;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _nullHitSafeTrailTargetPoint;
    [SerializeField] private Pool _impactPool;

    private void OnEnable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit += ShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit -= ShootPerformed;
    }

    private void ShootPerformed(RaycastHit? hit)
    {
        Vector3 point;
        Vector3 normal;
        if (hit == null)
        {
            point = _nullHitSafeTrailTargetPoint.position;
        }
        else
        {
            point = (Vector3) hit?.point;
            normal = (Vector3) hit?.normal;
            PoolObject poolObject = _impactPool.GetFreeElement(point, Quaternion.LookRotation(normal));
            poolObject.transform.position -= transform.forward * 0.2f;
        }
    }
}