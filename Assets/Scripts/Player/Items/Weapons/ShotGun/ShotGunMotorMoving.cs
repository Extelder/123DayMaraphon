using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotGunMotorMoving : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _defaultPosition;
    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;

    private Tween _downTween;
    private Tween _upTween;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.localPosition = _defaultPosition;
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            _downTween = transform.DOLocalMove(transform.localPosition - _offset, Random.Range(_minTime, _maxTime))
                .SetEase(Ease.Linear);
            yield return new WaitUntil(() => !_downTween.IsPlaying());
            _downTween.Kill();
            _upTween = transform.DOLocalMove(transform.localPosition + _offset, Random.Range(_minTime, _maxTime))
                .SetEase(Ease.Linear);
            yield return new WaitUntil(() => !_upTween.IsPlaying());
            _upTween.Kill();
        }
    }

    private void OnDisable()
    {
        _downTween.Kill();
        _upTween.Kill();
    }
}