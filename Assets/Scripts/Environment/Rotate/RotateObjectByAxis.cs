using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class RotateObjectByAxis : MonoBehaviour
{
    [SerializeField] private Vector3 _axis;
    [SerializeField] private Space _relativeTo;
    [SerializeField] private float _rotateSpeed;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => { transform.Rotate(_axis, _rotateSpeed * Time.deltaTime, _relativeTo); })
            .AddTo(_disposable);
    }


    private void OnDisable()
    {
        _disposable.Clear();
    }
}