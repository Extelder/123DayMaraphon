using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTrapableEnemyAnimator : PassiveEnemyAnimator
{
    [SerializeField] private string _trapAnimationBoolName;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_trapAnimationBoolName, false);
    }

    public void Trap()
    {
        SetTrueAnimationBoolWithDisableOthers(_trapAnimationBoolName);
    }
}
