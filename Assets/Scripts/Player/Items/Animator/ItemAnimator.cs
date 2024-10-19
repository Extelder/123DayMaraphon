using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : UnitAnimator
{
    [SerializeField] private string _moveAnimationBoolName, _shootAnimationBoolName;

    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_moveAnimationBoolName, false);
        SetAnimationBool(_shootAnimationBoolName, false);
    }

    public void Idle()
    {
        DisableAllBools();
    }

    public void Move()
    {
        SetTrueAnimationBoolWithDisableOthers(_moveAnimationBoolName);
    }

    public void Shoot()
    {
        SetTrueAnimationBoolWithDisableOthers(_shootAnimationBoolName);
    }
}