using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IceProjectile : PoolObject
{
    [SerializeField] private IceHitBox _iceHitBox;
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

    private void OnEnable()
    {
        Initiate();
    }

    public void Initiate()
    {
        _iceHitBox.Hitted += OnHitted;
        transform.eulerAngles = new Vector3(0, 0, 0);

        _collider.enabled = true;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(Vector3.down * _speed, ForceMode.Impulse);
        _collider.OnCollisionEnterAsObservable().Subscribe(other =>
        {
            Debug.LogError(other.gameObject);
            if (other.gameObject.TryGetComponent<PlayerHitBox>(out PlayerHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * _characterUpForce, ForceMode.Impulse);
                hitBox.TakeDamage(Damage);
                gameObject.SetActive(false);
                _collider.enabled = false;
                Triggered?.Invoke();
            }
            else
            {
                _collider.enabled = false;
            }
        }).AddTo(_disposable);
    }

    private void OnHitted()
    {
        _disposable.Clear();
    }

    private void OnDisable()
    {
        _iceHitBox.Hitted -= OnHitted;
        transform.eulerAngles = new Vector3(0, 0, 0);
        _disposable.Clear();
    }
}