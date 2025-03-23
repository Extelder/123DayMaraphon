using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusAnimator : UnitAnimator
{
    [SerializeField] private string _leftPunchTriggerName;
    [SerializeField] private string _rightPunchTriggerName;
    [SerializeField] private string _shootBoolName;
    [SerializeField] private string _moveUpTriggerName;
    [SerializeField] private string _shootUpBoolName;
    [SerializeField] private string _slamTriggerName;

    public void ShootUp()
    {
        SetTrueAnimationBoolWithDisableOthers(_shootUpBoolName);
    }

    public void MoveUp()
    {
        DisableAllBools();
        Debug.Log(_moveUpTriggerName);
        SetAnimationTrigger(_moveUpTriggerName);
    }

    public void Slam()
    {
        DisableAllBools();
        SetAnimationTrigger(_slamTriggerName);
    }

    public void LeftPunch()
    {
        DisableAllBools();
        SetAnimationTrigger(_leftPunchTriggerName);
    }

    public void Shoot()
    {
        SetTrueAnimationBoolWithDisableOthers(_shootBoolName);
    }

    public void RightPunch()
    {
        DisableAllBools();
        SetAnimationTrigger(_rightPunchTriggerName);
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_shootBoolName, false);
        SetAnimationBool(_shootUpBoolName, false);
        ResetAnimationTrigger(_rightPunchTriggerName);
        ResetAnimationTrigger(_leftPunchTriggerName);
        ResetAnimationTrigger(_moveUpTriggerName);
        ResetAnimationTrigger(_slamTriggerName);
    }
}