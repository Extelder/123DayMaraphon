using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaycastWeaponShoot : WeaponShoot
{
    public event Action<RaycastHit?> ShootPerformedWithRaycastHit;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(Camera.position, (Camera.forward + Camera.rotation * CurrentShootOffset) * Range);
    }

    public override void OnShootPerformed()
    {
        base.OnShootPerformed();
        CameraShakeInvoke();

        for (int i = 0; i < Weapon.HitsPerShot; i++)
        {
            CurrentShootOffset = Random.insideUnitCircle * Weapon.RandomRangeMultiplayer;

            if (GetHitColliderWithOffset(out Collider collider, CurrentShootOffset, out RaycastHit hit))
            {
                ShootPerformedWithRaycastHit?.Invoke(hit);
                Debug.Log(collider.gameObject.name);
                if (collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor weaponVisitor))
                {
                    Accept(weaponVisitor);
                }
            }
            else
            {
                ShootPerformedWithRaycastHit?.Invoke(null);
            }
        }
    }
}