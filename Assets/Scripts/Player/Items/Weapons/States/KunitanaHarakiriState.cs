using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunitanaHarakiriState : WeaponState
{
    [SerializeField] private string _harakiriAnimatorStringName;
    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private KunitanStateMachine _stateMachine;

    public event Action HarakiriPerformed;

    public override void Enter()
    {
        Animator.SetAnimationTrigger(_harakiriAnimatorStringName);
    }

    public void HarakiriAnimationEvent()
    {
        Animator.ResetAnimationTrigger(_harakiriAnimatorStringName);
        _bloodParticle.Play();
        _stateMachine.Idle();
        HarakiriPerformed?.Invoke();
    }

    public override void Exit()
    {
    }
}