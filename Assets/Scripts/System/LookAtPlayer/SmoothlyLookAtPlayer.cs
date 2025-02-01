using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class SmoothlyLookAtPlayer : MonoBehaviour
{
    
    [Inject] private PlayerCharacter _playerCharacter;
    [SerializeField] private float _turnSpeed;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private Transform _player;

    private void Awake()
    {
        _player = _playerCharacter.Transform;
    }

    private void OnEnable()
    {
        Observable.EveryUpdate().Subscribe(_ =>
            {
                Vector3 direction = (_player.position - transform.position).normalized;
                Quaternion goalRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, goalRotation.y, 0), _turnSpeed);
            })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
