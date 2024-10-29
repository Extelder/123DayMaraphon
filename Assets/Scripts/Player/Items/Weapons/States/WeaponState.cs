using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState : State
{
    public WeaponAnimator Animator;
    
    public abstract override void Enter();
}
