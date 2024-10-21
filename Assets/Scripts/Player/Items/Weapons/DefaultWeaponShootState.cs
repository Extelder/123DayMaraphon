using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefaultWeaponShootState : WeaponShootState
{
    [Inject] private PlayerInputs _playerInputs;

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
        if (_playerInputs.MainShooting)
            return;
        CanChanged = true;
        CanShoot = true;
        Animator.DisableAllBools();
    }

    private void OnDisable()
    {
        AnimationEndCanChanged();
    }
}