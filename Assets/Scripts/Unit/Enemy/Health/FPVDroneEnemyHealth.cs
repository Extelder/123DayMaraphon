using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVDroneEnemyHealth : Health
{
    public event Action Dead;
    public override void Death()
    {
        Dead?.Invoke();
    }
}
