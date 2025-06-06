using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WarmingUpAudio : MonoBehaviour
{
    [SerializeField] private float _upVolumeSpeed;

    [SerializeField] private PlayerWarmingUp _warming;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _warming.WarmingUpStarted += Started;
        _warming.WarmingUpEnded += Ended;
    }

    private void Ended()
    {
        Observable.EveryUpdate().Subscribe(_ =>
            {
                AudioListener.volume = Mathf.Lerp(AudioListener.volume, 1, _upVolumeSpeed * Time.deltaTime);
                if (AudioListener.volume >= 0.9f)
                {
                    AudioListener.volume = 1;
                    _disposable.Clear();
                }
            })
            .AddTo(_disposable);
    }

    private void Started()
    {
        AudioListener.volume = 0;
    }

    private void OnDisable()
    {
        _disposable?.Clear();
        _warming.WarmingUpStarted -= Started;
        _warming.WarmingUpEnded -= Ended;
    }
}