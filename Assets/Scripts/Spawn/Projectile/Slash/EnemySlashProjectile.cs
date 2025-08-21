using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class EnemySlashProjectile : PoolObject, ISlashProjectile
{
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float CharacterForce { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }

    public event Action Triggered;

    private Rigidbody _rigidbody;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Initiate(Vector3 targetPosition)
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.LookAt(targetPosition, transform.forward);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
        Collider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * CharacterForce, ForceMode.Impulse);
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