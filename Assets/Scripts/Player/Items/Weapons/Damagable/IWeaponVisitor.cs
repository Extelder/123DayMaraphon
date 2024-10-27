using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(WeaponShoot weaponShoot);
    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit);
    public void Visit(Projectile projectile);
}