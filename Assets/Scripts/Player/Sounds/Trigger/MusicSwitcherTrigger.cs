using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class MusicSwitcherTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _mainAudio;
    [SerializeField] private AudioClip _audioToSwitch;
    [SerializeField] private float _time;

    private CompositeDisposable _compositeDisposable;
    
    private int _defaultVolume;
    private Tween _changeVolumeTween;
    private Tween _switchAudioTween;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox playerHitBox))
        {
            ChangeAudioVolume();
        }
    }

    private void ChangeAudioVolume()
    {
        _changeVolumeTween = _mainAudio.DOFade(0, _time).OnComplete(() =>
        {
            SwitchAudio();
        });
    }

    private void SwitchAudio()
    {
        _mainAudio.clip = _audioToSwitch;
        _mainAudio.Play();
        _switchAudioTween = _mainAudio.DOFade(1, _time);
    }


    private void Update()
    {
        Debug.Log(_mainAudio.volume);
    }

    private void OnDisable()
    {
        _switchAudioTween?.Kill();
        _changeVolumeTween?.Kill();
    }
}
