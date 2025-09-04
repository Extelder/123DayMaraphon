using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CamerHeadBob : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationName;
    
    private CompositeDisposable _disposable = new CompositeDisposable();
    private void OnEnable()
    {
        PlayerCharacter.Instance.PlayerWalk.Moving.Subscribe(_ =>
        {
            _animator.SetBool(_animationName, _);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
