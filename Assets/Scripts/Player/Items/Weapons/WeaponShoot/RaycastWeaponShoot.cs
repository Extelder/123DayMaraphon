using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaycastWeaponShoot : WeaponShoot
{
    public event Action<RaycastHit?> ShootPerformedWithRaycastHit;

    private RaycastHit _hit;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(Camera.position, (Camera.forward + Camera.rotation * CurrentShootOffset) * Range);
    }

    public override void OnShootPerformed()
    {
        base.OnShootPerformed();
        CameraShakeInvoke();

        KunitanShoot.Instance.SetLastShoot(this);
        for (int i = 0; i < Weapon.HitsPerShot; i++)
        {
            CurrentShootOffset = Random.insideUnitCircle * Weapon.RandomRangeMultiplayer;

            if (GetHitColliderWithOffset(out Collider collider, CurrentShootOffset, out RaycastHit hit))
            {
                ShootPerformedWithRaycastHit?.Invoke(hit);
                if (collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor weaponVisitor))
                {
                    _hit = hit;
                    Accept(weaponVisitor);
                }
            }
            else
            {
                ShootPerformedWithRaycastHit?.Invoke(null);
            }
        }
    }

    public override void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this, _hit);
    }
}