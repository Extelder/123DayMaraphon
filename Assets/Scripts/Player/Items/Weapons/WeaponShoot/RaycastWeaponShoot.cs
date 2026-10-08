using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Тип выстрела для комбо: берст рейлгана — мультишот, дробь — buckshot, остальные рейкасты — hitscan.
public enum RaycastShotKind
{
    HitScan,
    Buckshot,
    Multishot
}

public class RaycastWeaponShoot : WeaponShoot
{
    public event Action<RaycastHit?> ShootPerformedWithRaycastHit;

    public bool Bursting { get; set; }
    public float BurstDamageMultiplier { get; set; } = 1f;

    public RaycastShotKind Kind
    {
        get
        {
            if (Bursting)
                return RaycastShotKind.Multishot;
            return Weapon != null && Weapon.HitsPerShot > 1 ? RaycastShotKind.Buckshot : RaycastShotKind.HitScan;
        }
    }

    public float Damage => Weapon.DamagePerHit * (Bursting ? BurstDamageMultiplier : 1f);

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

    public void OnKunitanShootPerformed()
    {
        HypeValue *= KunitanShoot.Instance.HitHypeMultiplier;
        HypeType = HypeType.KunitanDouble;
        OnShootPerformed();
        HypeValue /= KunitanShoot.Instance.HitHypeMultiplier;
        HypeType = HypeType.Kill;
    }

    public override void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this, _hit);
    }
}