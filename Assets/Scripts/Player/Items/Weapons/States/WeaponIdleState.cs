using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdleState : WeaponState
{
    public override void Enter()
    {
        Animator.Idle();
    }
}