using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSMoveUpState : State
{
    [SerializeField] private PusAnimator _animator;

    public override void Enter()
    {
        CanChanged = false;
        _animator.MoveUp();
    }

    public void MoveUpAnimationEnd()
    {
        CanChanged = true;
    }
}