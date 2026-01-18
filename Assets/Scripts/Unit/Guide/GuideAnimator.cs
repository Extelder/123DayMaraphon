using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAnimator : EnemyAnimator
{
    [SerializeField] private string _idle;
    public override void Idle()
    {
        SetTrueAnimationBoolWithDisableOthers(_idle);
    }

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_idle, false);
    }
}
