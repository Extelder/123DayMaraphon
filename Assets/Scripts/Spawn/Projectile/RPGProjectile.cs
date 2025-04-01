using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RPGProjectile : Projectile
{
    [SerializeField] private TrailRenderer _magnitableTrail;

    [SerializeField] private LayerMask _enemiesMask;
    [SerializeField] private LayerMask _ignoreEnemiesRaycastMask;

    [SerializeField] private float _searchingRange;
    [SerializeField] private float _maxDistance;

    [SerializeField] private Transform _projectileExplosionGFX;
    [SerializeField] private float _scaleFactor = 2;

    private Collider[] _enemiesColliders = new Collider[50];
    private List<ProjectileMagnitable> _magnitableColliders = new List<ProjectileMagnitable>();

    private float _defaultHypeValue;

    public override void Initiate(Vector3 targetPosition)
    {
        base.Initiate(targetPosition);
        _magnitableTrail.gameObject.SetActive(false);
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

            if (other.TryGetComponent<ProjectileMagnitable>(out ProjectileMagnitable ProjectileMagnitable))
            {
                if (Physics.Raycast(transform.position, (ProjectileMagnitable.transform.position - transform.position),
                    out RaycastHit hit, _maxDistance, ~_ignoreEnemiesRaycastMask))
                {
                    if (hit.collider.TryGetComponent<ProjectileMagnitable>(
                        out ProjectileMagnitable projectileMagnitable))
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
            HypeValue *= 2;
            PlayerTime.Instance.TimeStop(0.2f);
            Observable.EveryUpdate()
                .Subscribe(_ => { Rigidbody.MovePosition(projectileMagnitable.transform.position); }).AddTo(Disposable);
        }
    }

    public void ScaleProjectile()
    {
        ExplosionRange *= _scaleFactor;
        _projectileExplosionGFX.transform.localScale *= _scaleFactor;
    }
}