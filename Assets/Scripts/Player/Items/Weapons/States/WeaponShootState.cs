using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShootState : WeaponState
{
    public bool CanShoot = true;

    public abstract override void Enter();
}
