using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusFirstStageAnimator : UnitAnimator
{
    [SerializeField] private string _leftPunchTriggerName;
    [SerializeField] private string _rightPunchTriggerName;
    [SerializeField] private string _shootBoolName;

    public void LeftPunch()
    {
        DisableAllBools();
        SetAnimationTrigger(_leftPunchTriggerName);
    }

    public void Shoot()
    {
        DisableAllBools();
        SetAnimationBool(_shootBoolName, true);
    }

    public void RightPunch()
    {
        DisableAllBools();
        SetAnimationTrigger(_rightPunchTriggerName);
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_shootBoolName, false);
        ResetAnimationTrigger(_rightPunchTriggerName);
        ResetAnimationTrigger(_leftPunchTriggerName);
    }
}