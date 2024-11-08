using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

public class Projectile : PoolObject
{
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
    [SerializeField] private GameObject _projectileGFX;
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float ExplosionForce { get; private set; }
    [SerializeField] private float _explosionRange;
    [SerializeField] private bool _onlyPlayerHealth;
    [SerializeField] private ParticleSystem _explosiveParticle;
    [SerializeField] private float _speed;

    private Collider[] colliders = new Collider[50];
    private CompositeDisposable _disposable = new CompositeDisposable();
    private bool _explosived;

    public void Initiate(Vector3 targetPosition)
    {
        _projectileGFX.SetActive(true);
        _disposable?.Clear();
        transform.LookAt(targetPosition, transform.forward);
        Observable.EveryLateUpdate().Subscribe(_ =>
        {
            transform.position =
                Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - targetPosition) <= 2)
            {
                Explode();
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _explosived = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_onlyPlayerHealth)
            Explode();

        if (!other.TryGetComponent<PlayerMovement>(out PlayerMovement movement) && !_onlyPlayerHealth)
            Explode();
    }

    public void Explode()
    {
        if (_explosived)
            return;
        Debug.Log("Explosived");
        _cinemachineImpulseSource?.GenerateImpulse();
        _explosived = true;
        _disposable.Clear();
        Physics.OverlapSphereNonAlloc(transform.position, _explosionRange, colliders);

        foreach (var other in colliders)
        {
            if (!other)
                continue;

            if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
            {
                if (_onlyPlayerHealth)
                    continue;
                Accept(visitor);
                continue;
            }

            if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox playerHitBox))
            {
                if (_onlyPlayerHealth)
                {
                    playerHitBox.TakeDamage(Damage);
                    return;
                }
            }

            if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, _explosionRange);
            }

            if (other.TryGetComponent<Health>(out Health health))
            {
                if (health is PlayerHealth && _onlyPlayerHealth)
                {
                    health.TakeDamage(Damage / Vector3.SqrMagnitude(transform.position - health.transform.position));
                    continue;
                }

                if (health != this)
                    health.TakeDamage(Damage / Vector3.SqrMagnitude(transform.position - health.transform.position));
            }
        }

        _explosiveParticle?.Play();
        _projectileGFX.SetActive(false);
        Invoke(nameof(ReturnToPool), ReturnToPoolDelay);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRange);
    }

    public void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }
}