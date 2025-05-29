using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Magnitable : MonoBehaviour
{
    [SerializeField] private float _magnitSpeed = 0.3f;

    public bool Moving { get; private set; }

    private CompositeDisposable _disposable = new CompositeDisposable();

    public virtual void OnEnable()
    {
        Moving = false;
    }

    public virtual void Magnit(Transform point, Action OnMagnited = null)
    {
        if (Moving)
            return;
        Moving = true;

        Debug.Log("Magniniting");

        Observable.EveryUpdate().Subscribe(_ =>
        {
            transform.position =
                Vector3.MoveTowards(transform.position, point.position, _magnitSpeed * Time.deltaTime);
        }).AddTo(_disposable);
    }

    public virtual void OnDisable()
    {
        _disposable.Clear();
    }
}