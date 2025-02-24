using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyAnimator : UnitAnimator
{
    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
    }

    public void Idle()
    {
        DisableAllBools();
    }

    public void DisableAnimator()
    {
        Animator.enabled = false;
    }
}
