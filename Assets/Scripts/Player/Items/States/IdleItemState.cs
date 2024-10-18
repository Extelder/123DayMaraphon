using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleItemState : ItemState
{
    public override void Enter()
    {
        Animator.Idle();
    }
}