using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class PermanentLevelChackPointTrigger : MonoBehaviour
{
    [Inject] private PlayerCharacter _playerCharacter;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnTriggerEnter(Collider other)
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _playerCharacter.PlayerLevelCheckPoint.SetNewCheckPoint(transform.position);
        }).AddTo(_disposable);
    }


    private void OnDisable()
    {
        _disposable?.Clear();
    }

    private void OnTriggerExit(Collider other)
    {
        _disposable?.Clear();
    }
}