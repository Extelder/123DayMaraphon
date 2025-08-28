using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WhaleMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private PlayerTrigger _playerTrigger;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _playerTrigger.Triggered += OnTriggered;
    }

    private void OnTriggered()
    {
        _playerTrigger.Triggered -= OnTriggered;
        Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            transform.Translate(transform.forward * _speed * Time.fixedDeltaTime, Space.World);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable?.Clear();
        _playerTrigger.Triggered -= OnTriggered;
    }
}