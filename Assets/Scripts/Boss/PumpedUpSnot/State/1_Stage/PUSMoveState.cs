using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PUSMoveState : State
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _pus;
    [SerializeField] private float _moveDuration;

    private Tween _tween;

    public override void Enter()
    {
        _tween.Kill();
        Vector3 point = _points[Random.Range(0, _points.Length)].position;
        _pus.DOMove(point, _moveDuration);
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}