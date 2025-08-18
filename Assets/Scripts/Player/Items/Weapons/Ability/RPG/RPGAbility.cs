using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class RPGAbility : WeaponAbility
{
    [SerializeField] private Transform _slashSpawnPoint;
    [SerializeField] private Transform _camera;
    [SerializeField] private double _cooldown;
    [SerializeField] private GameObject _indicator;

    [Inject] private Pools _pools;
    
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        Observable.Interval(TimeSpan.FromSeconds(_cooldown)).Subscribe(_ =>
        {
            PoolObject instance = _pools.PlayerSlashProjectilePool.GetFreeElement(_slashSpawnPoint.position, _camera.transform.rotation);
            instance.GetComponent<PlayerSlashProjectile>().Initiate();
        }).AddTo(_disposable);
    }

    public void StopAbility()
    {
        _indicator.SetActive(false);
        _disposable?.Clear();
    }

    protected override void OnDisableVirtual()
    {
        base.OnDisableVirtual();
        _indicator.SetActive(false);
        _disposable.Clear();
    }
}