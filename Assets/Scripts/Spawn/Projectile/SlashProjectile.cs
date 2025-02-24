using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class SlashProjectile : PoolObject
{
    [field: SerializeField] public float Damage { get; private set; }
    [SerializeField] private float _speed;
    [SerializeField] private float _characterUpForce;

    public event Action Triggered;

    private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Initiate(Vector3 targetPosition)
    {
        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.LookAt(targetPosition, transform.forward);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
        _collider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * _characterUpForce, ForceMode.Impulse);
                hitBox.TakeDamage(Damage);
                Triggered?.Invoke();
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        _disposable.Clear();
    }
}