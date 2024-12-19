using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float _minAsteroidMoveSpeed;
    [SerializeField] private float _maxAsteroidMoveSpeed;
    [SerializeField] private float _scaleChangeFactor;
    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Observable.EveryUpdate().Subscribe(_ =>
            {
                transform.Translate(transform.forward * Random.Range(_minAsteroidMoveSpeed, _maxAsteroidMoveSpeed) * Time.deltaTime);
                Vector3 newScale = transform.localScale;
                newScale *= _scaleChangeFactor;
                transform.localScale = newScale;
            })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}