using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

public class Projectile : PoolObject, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; }= 0.1f;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _projectileGFX;
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float ExplosionForce { get; private set; }
    [SerializeField] private float _explosionRange;
    [SerializeField] private bool _onlyPlayerHealth;
    [SerializeField] private ParticleSystem _explosiveParticle;
    [SerializeField] private float _speed;

    private Collider[] colliders = new Collider[50];
    private Collider _collider;
    private bool _explosived;
    private bool _useGravity;

    private float _defaultDamage;

    private float _trailTime;

    public event Action Exploded;

    [SerializeField] private Rigidbody _rigidbody;

    private void Awake()
    {
        _useGravity = _rigidbody.useGravity;
        _defaultDamage = Damage;
        _trailTime = _trail.time;
        _collider = GetComponent<Collider>();
    }

    public void Initiate(Vector3 targetPosition)
    {
        StopAllCoroutines();
        _collider.enabled = true;
        _projectileGFX.SetActive(true);
        _trail.enabled = true;
        _trail.time = -1;
        _rigidbody.useGravity = _useGravity;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        StartCoroutine(WaitingForFrame());
        transform.LookAt(targetPosition, transform.forward);
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    private IEnumerator WaitingForFrame()
    {
        yield return new WaitForEndOfFrame();
        _trail.time = _trailTime;
    }

    private void OnDisable()
    {
        _trail.time = 0;
        _trail.enabled = false;
        _explosived = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
            return;
        if (_onlyPlayerHealth)
            Explode();

        if (!other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement movement) && !_onlyPlayerHealth)
            Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out Projectile projectile))
            return;
        if (_onlyPlayerHealth)
            Explode();

        if (!other.TryGetComponent<PlayerMovement>(out PlayerMovement movement) && !_onlyPlayerHealth)
            Explode();
    }

    public void Explode()
    {
        Damage = _defaultDamage;
        Explode(1);
    }

    public void Explode(float damageMultiplier)
    {
        Damage *= damageMultiplier;
        if (_explosived)
            return;
        _collider.enabled = false;
        _trail.time = 0;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = new Vector3(0, 0, 0);

        _cinemachineImpulseSource?.GenerateImpulse();
        _explosived = true;
        colliders = new Collider[35];
        Physics.OverlapSphereNonAlloc(transform.position, _explosionRange, colliders, _layerMask);

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
                    playerHitBox.TakeDamage(Damage * damageMultiplier);
                    continue;
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
                    health.TakeDamage(Damage * damageMultiplier /
                                      Vector3.SqrMagnitude(transform.position - health.transform.position));
                    continue;
                }

                if (health != this)
                    health.TakeDamage(Damage * damageMultiplier /
                                      Vector3.SqrMagnitude(transform.position - health.transform.position));
            }
        }


        Exploded?.Invoke();
        _explosiveParticle?.Play();
        _projectileGFX.SetActive(false);
        Invoke(nameof(ReturnToPool), ReturnToPoolDelay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRange);
    }

    public void HitExplode()
    {
        Explode(5);
        PlayerTime.Instance.TimeStop(0.2f);
    }

    public void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }
}