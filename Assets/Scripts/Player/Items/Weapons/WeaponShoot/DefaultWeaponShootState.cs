using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefaultWeaponShootState : WeaponShootState
{
    [Inject] private PlayerInputs _playerInputs;

    public event Action ShootPerformed;

    private bool _alreadyShooting;

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


    public void AnimationEndStartChecking()
    {
        _alreadyShooting = false;
        StopAllCoroutines();
        StartCoroutine(AnimationEndChecking());
    }

    public void AnimationEndStopChecking()
    {
        StopAllCoroutines();

        if (_alreadyShooting)
            return;

        CanChanged = true;
        CanShoot = true;
    }

    private IEnumerator AnimationEndChecking()
    {
        while (true)
        {
            if (_playerInputs.PlayerWeaponInputs.MainShooting)
            {
                _alreadyShooting = true;
                Animator.Shoot();
                yield break;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }
}