using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using UniRx;

public class PlayerTextTrigger : PlayerTrigger
{
    private CompositeDisposable _disposable = new CompositeDisposable();
    [SerializeField] private Pool _confettiPool;
    [SerializeField] private Pool _textPool;
    [Inject] private PlayerCharacter _character;

    [SerializeField] private float _textMoveSpeed;
    [SerializeField] private Transform _spawnPlace;
    [SerializeField] private Transform _particleSpawnPlace;
    [SerializeField] private float _scaleChangeFactor;
    
    public event Action PlayerTriggered;
    public event Action PlayerUntriggered;
    public override void Triggered()
    {
        var instance = _textPool.GetFreeElement(_spawnPlace.position);
        Observable.EveryUpdate().Subscribe(_ =>
        {
            instance.transform.position = Vector3.Lerp(instance.transform.position,
                _character.PlaceForTextLerping.transform.position, _textMoveSpeed * Time.deltaTime);
            ChangeScale(instance);
        }).AddTo(_disposable);
        _confettiPool.GetFreeElement(_particleSpawnPlace.position);
        PlayerTriggered?.Invoke();
    }

    private void ChangeScale(PoolObject instance)
    {
        Vector3 newScale = transform.localScale;
        newScale *= _scaleChangeFactor;
        instance.transform.localScale = newScale;
    }

    private void OnDisable()
    {
        _disposable.Clear();
        PlayerUntriggered?.Invoke();
        DestroyComponentAfterTriggered = true;
        DestroyGameObjectAfterTriggered = true;
    }
}
