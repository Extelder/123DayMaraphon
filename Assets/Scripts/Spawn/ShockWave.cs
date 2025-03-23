using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private float _characterUpForce;
    [SerializeField] private float Damage;

    [SerializeField] private GameObject _collider;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _collider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * _characterUpForce, ForceMode.Impulse);
                hitBox.TakeDamage(Damage);
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        _disposable.Clear();
    }
}