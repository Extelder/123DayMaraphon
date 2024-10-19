using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShootState : ItemState
{
    public bool CanShoot = true;

    public abstract override void Enter();
}
