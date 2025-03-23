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
    [SerializeField] private PUSNearbyCheck _pusNearbyCheck;

    private Tween _tween;

    public override void Enter()
    {
        Debug.LogError("Enter Move");
        _pusNearbyCheck.enabled = false;
        _tween.Kill();
        Vector3 point = _points[Random.Range(0, _points.Length)].position;
        _pus.DOMove(point, _moveDuration).OnComplete(() => { _pusNearbyCheck.enabled = true; });
    }

    public override void Exit()
    {
        _pusNearbyCheck.enabled = true;
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}