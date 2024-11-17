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

    private CompositeDisposable _positionDisposable = new CompositeDisposable();
    private CompositeDisposable _rotationDisposable = new CompositeDisposable();

    private Tween _positionTween;
    private Tween _rotationTween;

    public override void Lerp()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _positionTween = Target.DOMove(Point.position, _positionSpeed)
                .SetEase(Ease.Flash)
                .OnComplete(() => { _positionDisposable.Clear(); });
        }).AddTo(_positionDisposable);

        Observable.EveryUpdate().Subscribe(_ =>
        {
            _rotationTween = Target.DORotate(Point.localEulerAngles, _rotationSpeed)
                .SetEase(Ease.Flash)
                .OnComplete(() => { _rotationDisposable.Clear(); });
        }).AddTo(_rotationDisposable);
    }

    private void OnDisable()
    {
        _positionDisposable.Clear();
        _rotationDisposable.Clear();

        _positionTween.Kill();
        _rotationTween.Kill();
    }
}