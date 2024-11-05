using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class FlyEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _autoDestroyDelay;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private ProjectileHealth _health;

    [Inject] private Pools _pools;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Invoke(nameof(OnDisable), _autoDestroyDelay);

        _health.Dead += OnDisable;

        Observable.EveryUpdate().Subscribe(_ =>
        {
            transform.LookAt(_playerTransform);
            transform.position = Vector3.Slerp(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _health.Dead -= OnDisable;
        _disposable.Clear();
        _pools.BloodExplodePool.GetFreeElement(transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(_damage);
            _pools.BloodExplodePool.GetFreeElement(transform.position, Quaternion.identity);
            OnDisable();
        }
    }
}