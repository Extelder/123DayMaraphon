using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        Animator.Idle();
    }
}
