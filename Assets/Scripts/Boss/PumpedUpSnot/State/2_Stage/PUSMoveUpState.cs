using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PUSMoveUpState : State
{
    [SerializeField] private PusAnimator _animator;
    [SerializeField] private Transform _pus;
    [SerializeField] private PUSShootState _shootState;

    [SerializeField] private Transform _point;

    private Tween _tween;

    public override void Enter()
    {
        CanChanged = false;
        _animator.MoveUp();
        _shootState.CanChanged = true;
        _shootState.Exit();
        _tween = _pus.DOMove(_point.position, 1);
    }

    public void MoveUpAnimationEnd()
    {
        CanChanged = true;

        _tween.Kill();
    }

    private void OnDisable()
    {
        _tween.Kill();
    }
}