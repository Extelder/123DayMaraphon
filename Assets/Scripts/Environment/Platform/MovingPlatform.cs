using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPlatform : PlayerTrigger
{
    
    [SerializeField] private GameObject _platform;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _lerpingTime;

    private Tween _tween;
    public override void OnTriggered()
    {
        _tween = _platform.transform.DOMove(_endPoint.position, _lerpingTime).SetSpeedBased().SetEase(Ease.Linear);
    }
}
