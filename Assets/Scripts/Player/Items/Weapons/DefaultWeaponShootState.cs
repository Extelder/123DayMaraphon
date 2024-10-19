using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeaponShootState : WeaponShootState
{
    public event Action ShootPerformed;

    public override void Enter()
    {
        CanChanged = false;
        CanShoot = false;
        Animator.Shoot();
    }

    public void PerformShoot()
    {
        ShootPerformed?.Invoke();
    }

    public void AnimationEndCanChanged()
    {
        CanChanged = true;
        CanShoot = true;
        Animator.DisableAllBools();
    }

    private void OnDisable()
    {
        AnimationEndCanChanged();
    }
}