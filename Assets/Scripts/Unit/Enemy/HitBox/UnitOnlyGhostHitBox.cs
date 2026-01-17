using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOnlyGhostHitBox : UnitHitBox
{
    public override void Visit(KunitanaUltimateAttack kunitanaUltimateAttack)
    {
    }

    public override void Visit(PlayerSlashProjectile slashProjectile)
    {
    }

    public override void Visit(WeaponShoot weaponShoot)
    {
    }

    public override void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
    }

    public override void Visit(KunitanShoot kunitanShoot)
    {
    }

    public override void Visit(Projectile projectile)
    {
    }
}
