using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MoveBySinusoid : MonoBehaviour
{
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    private void Start()
    {
        Move();
    }

    private void Move()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            Vector3 newPosition = transform.position;
            newPosition.y += Mathf.Sin(Time.time) * Time.deltaTime;
            transform.position = newPosition;
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
