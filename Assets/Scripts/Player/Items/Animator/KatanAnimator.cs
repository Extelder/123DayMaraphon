using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanAnimator : ItemAnimator
{
    [SerializeField] private string _moveAnimationBoolName, _shootAnimationBoolName, _blockAnimationBoolName;

    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_moveAnimationBoolName, false);
        SetAnimationBool(_shootAnimationBoolName, false);
        SetAnimationBool(_blockAnimationBoolName, false);
    }

    public void Block()
    {
        SetTrueAnimationBoolWithDisableOthers(_blockAnimationBoolName);
    }
}