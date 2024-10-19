using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class OverlapShootState : WeaponShoot
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private int _maxColliders;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, Range);
    }

    public override void OnShootPerformed()
    {
        base.OnShootPerformed();
        CameraShakeInvoke();

        for (int i = 0; i < Weapon.HitsPerShot; i++)
        {
            Collider[] colliders = new Collider[_maxColliders];
            Physics.OverlapSphereNonAlloc(_attackPoint.position, Range, colliders, ~ToIgnore);

            foreach (var other in colliders)
            {
                if (other == null)
                    continue;
                if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor weaponVisitor))
                {
                    Accept(weaponVisitor);
                }
            }
        }
    }
}