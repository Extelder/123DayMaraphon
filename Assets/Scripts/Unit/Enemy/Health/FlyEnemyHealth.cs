using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyHealth : Health
{
    [SerializeField] private FlyDefaultEnemyTrappable _enemyTrappable;
    public event Action Dead;
    public override void Death()
    {
        _enemyTrappable.OnTrapped();
        Dead?.Invoke();
    }
}
