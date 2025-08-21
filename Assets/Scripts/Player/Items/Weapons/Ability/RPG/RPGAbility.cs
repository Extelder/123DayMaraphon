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
    [SerializeField] private AudioSource _abilitySound;

    [Inject] private Pools _pools;
    
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        StopAbility();
        _abilitySound.Play();
        Observable.Interval(TimeSpan.FromSeconds(_cooldown)).Subscribe(_ =>
        {
            PoolObject instance = _pools.PlayerSlashProjectilePool.GetFreeElement(_slashSpawnPoint.position, _camera.transform.rotation);
            instance.GetComponent<PlayerSlashProjectile>().Initiate();
        }).AddTo(_disposable);
    }

    public void StopAbility()
    {
        _disposable?.Clear();
        _indicator.SetActive(false);
    }

    protected override void OnDisableVirtual()
    {
        base.OnDisableVirtual();
        StopAbility();
    }
}