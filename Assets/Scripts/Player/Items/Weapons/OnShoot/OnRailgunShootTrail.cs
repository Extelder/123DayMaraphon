using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRailgunShootTrail : OnRaycastWeaponShootTrail
{
    public override void SetPool()
    {
        pool = Pool.RailgunTrailPool;
    }
}
