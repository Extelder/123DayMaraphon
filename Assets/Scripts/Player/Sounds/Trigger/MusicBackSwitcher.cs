using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicBackSwitcher : MonoBehaviour
{
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _mainAudio;
    [SerializeField] private float _time;

    private Tween _changeVolumeTween;
    private Tween _switchAudioTween;

    public void ChangeMusicVolume()
    {
        _changeVolumeTween = _audioSource.DOFade(0, _time).OnComplete(() =>
        {
            SwitchAudio();
        });
    }

    private void SwitchAudio()
    {
        _audioSource.clip = _mainAudio;
        _audioSource.Play();
        _switchAudioTween = _audioSource.DOFade(1, _time);
    }

    private void OnDisable()
    {
        _changeVolumeTween.Kill();
        _switchAudioTween.Kill();
    }
}
