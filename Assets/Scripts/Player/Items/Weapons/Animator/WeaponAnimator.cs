using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : UnitAnimator
{
    [SerializeField] private string _shootAnimationTriggerName;

    private void Start()
    {
        Idle();
    }

    public override void DisableAllBools()
    {
        SetAnimationBool(_shootAnimationTriggerName, false);
    }

    public void Idle()
    {
        DisableAllBools();
    }

    public void Shoot()
    {
        SetAnimationTrigger(_shootAnimationTriggerName);
    }
}
