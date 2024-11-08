using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class LookAtPlayer : MonoBehaviour
{
    [Inject] private PlayerCharacter _playerCharacter;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private Transform _player;

    private void Awake()
    {
        _player = _playerCharacter.Transform;
    }

    private void OnEnable()
    {
        Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_player.position); })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}