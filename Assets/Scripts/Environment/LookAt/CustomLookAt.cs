using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public enum Direction
{
    Forward,
    Right,
    Left,
    Backward,
    Up,
    Down
}

public class CustomLookAt : MonoBehaviour
{
    [SerializeField] private Direction _direction;
    [SerializeField] private Transform _lookAtTransform;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        switch (_direction)
        {
            case Direction.Forward:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, transform.forward); })
                    .AddTo(_disposable);
                break;
            case Direction.Right:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, transform.right); })
                    .AddTo(_disposable);
                break;
            case Direction.Left:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, -transform.right); })
                    .AddTo(_disposable);
                break;
            case Direction.Backward:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, -transform.forward); })
                    .AddTo(_disposable);
                break;
            case Direction.Up:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, transform.up); })
                    .AddTo(_disposable);
                break;
            case Direction.Down:
                Observable.EveryUpdate().Subscribe(_ => { transform.LookAt(_lookAtTransform, -transform.up); })
                    .AddTo(_disposable);
                break;
        }
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}