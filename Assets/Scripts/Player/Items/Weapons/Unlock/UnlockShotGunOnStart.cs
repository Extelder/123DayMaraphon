using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockShotGunOnStart : WeaponUnlocker
{
    private void Start()
    {
        Invoke(nameof(UnlockWithDelay), 3f);
    }

    private void UnlockWithDelay()
    {
        UnlockShotGun();
    }
}