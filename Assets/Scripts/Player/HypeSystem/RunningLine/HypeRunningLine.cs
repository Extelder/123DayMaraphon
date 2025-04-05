using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class HypeRunningLine : PoolObject
{
    [SerializeField] private PlayerHypeRunningLine _runningLine;
    [SerializeField] private float _speed;
    [SerializeField] private TextMeshProUGUI _text;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public string CurrentHype { get; private set; }

    private void OnEnable()
    {
        CurrentHype = _runningLine.CurrentHypeName;
        _text.text = CurrentHype;
        Observable.EveryUpdate().Subscribe(_ =>
            {
                transform.Translate(transform.right * _speed * Time.deltaTime, Space.World);
            })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}