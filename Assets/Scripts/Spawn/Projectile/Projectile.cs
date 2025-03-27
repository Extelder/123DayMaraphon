using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class Projectile : PoolObject, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; }= 0.1f;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private TrailRenderer _magnitableTrail;
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private LayerMask _enemiesMask;
    [SerializeField] private LayerMask _ignoreEnemiesRaycastMask;
    [SerializeField] private GameObject _projectileGFX;
    [SerializeField] private float _searchingRange;
    [SerializeField] private float _maxDistance;
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float ExplosionForce { get; private set; }
    [field: SerializeField] public float ExplosionRange { get; set; }
    [SerializeField] private bool _onlyPlayerHealth;
    [SerializeField] private ParticleSystem _explosiveParticle;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _projectileExplosionGFX;
    [SerializeField] private float _scaleFactor = 2;

    private Collider[] _colliders = new Collider[50];
    private Collider[] _enemiesColliders = new Collider[50];
    private List<ProjectileMagnitable> _magnitableColliders = new List<ProjectileMagnitable>(); 
    private Collider _collider;
    private bool _explosived;
    private bool _useGravity;
    private CompositeDisposable _disposable = new CompositeDisposable();

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
        _magnitableTrail.gameObject.SetActive(false);
    }

    private IEnumerator WaitingForFrame()
    {
        yield return new WaitForEndOfFrame();
        _trail.time = _trailTime;
    }

    private void OnDisable()
    {
        _disposable.Clear();
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
        _disposable.Clear();
        Damage *= damageMultiplier;
        if (_explosived)
            return;
        _collider.enabled = false;
        _trail.time = 0;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = new Vector3(0, 0, 0);

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

    public void SearchNearestEnemy()
    {
        _magnitableColliders.Clear();
        Physics.OverlapSphereNonAlloc(transform.position, _searchingRange, _enemiesColliders, _enemiesMask);
        foreach (var other in _enemiesColliders)
        {
            if (other == null)
            {
                continue;
            }
            if(other.TryGetComponent<ProjectileMagnitable>(out ProjectileMagnitable ProjectileMagnitable))
            {
                if (Physics.Raycast(transform.position, (ProjectileMagnitable.transform.position - transform.position),
                    out RaycastHit hit , _maxDistance, ~_ignoreEnemiesRaycastMask))
                {
                    if (hit.collider.TryGetComponent<ProjectileMagnitable>(out ProjectileMagnitable projectileMagnitable))
                    {
                        projectileMagnitable.Distance = hit.distance;
                        _magnitableColliders.Add(ProjectileMagnitable);
                    }
                }
            }
        }

        ProjectileMagnitable currentProjectileMagnitable = null;
        float minDistance = Single.PositiveInfinity;
        foreach (var other in _magnitableColliders)
        {
            if (other.Distance < minDistance)
            {
                currentProjectileMagnitable = other;
                minDistance = other.Distance;
            }
        }
        MagnitProjectileToEnemy(currentProjectileMagnitable);
    }

    private void MagnitProjectileToEnemy(ProjectileMagnitable projectileMagnitable)
    {
        if (projectileMagnitable == null)
        {
            ScaleProjectile();
            HitExplode();
        }
        else
        {
            _magnitableTrail.gameObject.SetActive(true);
            PlayerTime.Instance.TimeStop(0.2f);
            Observable.EveryUpdate().Subscribe(_ =>
            {
                _rigidbody.MovePosition(projectileMagnitable.transform.position);
            }).AddTo(_disposable);
        }
    }
    
    public void ScaleProjectile()
    {
        ExplosionRange *= _scaleFactor;
        _projectileExplosionGFX.transform.localScale *= _scaleFactor;
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