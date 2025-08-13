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

    [Inject] private Pools _pools;
    
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        Observable.Interval(TimeSpan.FromSeconds(_cooldown)).Subscribe(_ =>
        {
            Debug.Log("dwdaw");
            PoolObject instance = _pools.PlayerSlashProjectilePool.GetFreeElement(_slashSpawnPoint.position, _camera.transform.rotation);
            instance.GetComponent<PlayerSlashProjectile>().Initiate();
        }).AddTo(_disposable);
    }

    protected override void OnDisableVirtual()
    {
        base.OnDisableVirtual();
        _disposable.Clear();
    }
}