using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemState : State
{
    public ItemAnimator Animator;
    
    public abstract override void Enter();
}
