using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _idleAnimationBoolName, _moveAnimationBoolName, _shootAnimationBoolName;

    private void Start()
    {
        Idle();
    }

    public virtual void DisableAllBools()
    {
        _animator.SetBool(_idleAnimationBoolName, false);
        _animator.SetBool(_moveAnimationBoolName, false);
        _animator.SetBool(_shootAnimationBoolName, false);
    }

    public void Idle()
    {
        SetAnimationBoolWithDisableOthers(_idleAnimationBoolName);
    }

    public void Move()
    {
        SetAnimationBoolWithDisableOthers(_moveAnimationBoolName);
    }

    public void Shoot()
    {
        SetAnimationBoolWithDisableOthers(_shootAnimationBoolName);
    }

    public void SetAnimationBoolWithDisableOthers(string name)
    {
        DisableAllBools();
        SetAnimationBool(name);
    }

    public void SetAnimationBool(string name)
    {
        _animator.SetBool(name, true);
    }
}