using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LookAtPlayerSingletone : MonoBehaviour
{
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    private void OnEnable()
    {
        Transform player = PlayerCharacter.Instance.Transform;
        Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(player.position); })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
