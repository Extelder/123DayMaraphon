using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : UnitAnimator
{
    [SerializeField] private string _moveAnimationBoolName, _attackAnimationBoolName, _takeDamageAnimationTriggerName;

    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_moveAnimationBoolName, false);
        SetAnimationBool(_attackAnimationBoolName, false);
    }

    public void Idle()
    {
        DisableAllBools();
    }

    public void Move()
    {
        SetTrueAnimationBoolWithDisableOthers(_moveAnimationBoolName);
    }

    public void Attack()
    {
        SetTrueAnimationBoolWithDisableOthers(_attackAnimationBoolName);
    }

    public void TakeDamage()
    {
        DisableAllBools();
        Animator.SetTrigger(_takeDamageAnimationTriggerName);
    }

    public void DisableAnimator()
    {
        Animator.enabled = false;
    }
}