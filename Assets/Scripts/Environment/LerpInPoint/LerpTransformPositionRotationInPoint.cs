using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class LerpTransformPositionRotationInPoint : LerpInPoint
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _positionSpeed;

    private Tween _positionTween;
    private Tween _rotationTween;

    public override void Lerp()
    {
        _positionTween = Target.DOMove(Point.position, _positionSpeed)
                .SetEase(Ease.Flash);

        _rotationTween = Target.DORotate(Point.localEulerAngles, _rotationSpeed)
                .SetEase(Ease.Flash);
    }

    private void OnDisable()
    {
        _positionTween.Kill();
        _rotationTween.Kill();
    }
}