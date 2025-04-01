using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class Projectile : PoolObjectTimeScalable, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; }= 0.1f;
    [field: SerializeField] public float DefaultHypeValue { get; set; }= 0.1f;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _projectileGFX;
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float ExplosionForce { get; private set; }
    [field: SerializeField] public float ExplosionRange { get; set; }
    [SerializeField] private bool _onlyPlayerHealth;
    [SerializeField] private ParticleSystem _explosiveParticle;
    [SerializeField] private float _speed;

    private Collider[] _colliders = new Collider[50];
    private Collider _collider;
    private bool _explosived;
    private bool _useGravity;
    public CompositeDisposable Disposable { get; private set; }= new CompositeDisposable();

    private float _defaultDamage;

    private float _trailTime;

    public event Action Exploded;
    public event Action Initiated;

    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        DefaultHypeValue = HypeValue;
        _useGravity = Rigidbody.useGravity;
        _defaultDamage = Damage;
        _trailTime = _trail.time;
        _collider = GetComponent<Collider>();
    }

    public virtual void Initiate(Vector3 targetPosition)
    {
        StopAllCoroutines();
        HypeValue = DefaultHypeValue;
        _collider.enabled = true;
        _projectileGFX.SetActive(true);
        _trail.enabled = true;
        _trail.time = -1;
        Rigidbody.useGravity = _useGravity;
        Rigidbody.velocity = new Vector3(0, 0, 0);
        StartCoroutine(WaitingForFrame());
        transform.LookAt(targetPosition, transform.forward);
        Rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
        Initiated?.Invoke();
    }

    private IEnumerator WaitingForFrame()
    {
        yield return new WaitForEndOfFrame();
        _trail.time = _trailTime;
    }

    private void OnDisable()
    {
        Disposable.Clear();
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
        Disposable.Clear();
        Damage *= damageMultiplier;
        if (_explosived)
            return;
        _collider.enabled = false;
        _trail.time = 0;

        Rigidbody.useGravity = false;
        Rigidbody.velocity = new Vector3(0, 0, 0);

        _cinemachineImpulseSource?.GenerateImpulse();
        _explosived = true;
        _colliders = new Collider[35];
        Physics.OverlapSphereNonAlloc(transform.position, ExplosionRange, _colliders, _layerMask);

        foreach (var other in _colliders)
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
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRange);
            }
        }


        Exploded?.Invoke();
        _explosiveParticle?.Play();
        _projectileGFX.SetActive(false);
        Invoke(nameof(ReturnToPool), ReturnToPoolDelay);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRange);
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