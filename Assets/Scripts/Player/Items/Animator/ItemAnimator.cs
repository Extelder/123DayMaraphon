using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : UnitAnimator
{
    [SerializeField] private string _shootAnimationBoolName;

    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_shootAnimationBoolName, false);
    }

    public void Idle()
    {
        DisableAllBools();
    }

    public void Shoot()
    {
        SetTrueAnimationBoolWithDisableOthers(_shootAnimationBoolName);
    }
}