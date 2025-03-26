using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IceProjectile : PoolObject
{
    [SerializeField] private GameObject _parent;
    [field: SerializeField] public float Damage { get; private set; }
    [SerializeField] private float _speed;
    [SerializeField] private float _characterUpForce;

    private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Initiate();
    }

    public void Initiate()
    {
        transform.eulerAngles = new Vector3(-90, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);

        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(Vector3.down * _speed, ForceMode.Impulse);
        _collider.OnCollisionEnterAsObservable().Subscribe(other =>
        {
            if (other.gameObject.TryGetComponent<PlayerHitBox>(out PlayerHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * _characterUpForce, ForceMode.Impulse);
                hitBox.TakeDamage(Damage);
                _parent.SetActive(false);
                _disposable.Clear();
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}