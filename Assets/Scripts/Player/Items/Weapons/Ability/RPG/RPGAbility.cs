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
    [SerializeField] private GameObject _indicator;
    [SerializeField] private AudioSource _abilitySound;

    [Header("Cross wave")]
    [SerializeField] private float _crossSize = 1f;
    [SerializeField] private float _crossLifetime = 10f;
    [SerializeField] private float _crossDamageMultiplier = 4f;
    [SerializeField] private Color _crossColor = new Color(0.25f, 0.6f, 2.4f, 1f);

    [Inject] private Pools _pools;
    
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        StopAbility();
        _abilitySound.Play();

        // Вместо потока волн — одна большая синяя волна-крест.
        PoolObject instance = _pools.PlayerSlashProjectilePool.GetFreeElement(_slashSpawnPoint.position,
            _camera.transform.rotation);
        PlayerSlashProjectile slash = instance.GetComponent<PlayerSlashProjectile>();
        slash.Initiate();
        slash.Damage *= _crossDamageMultiplier;
        CrossSlash.Apply(slash, _crossSize, _crossColor, _crossLifetime);

        ComboFx.FlashLight(_slashSpawnPoint.position, new Color(0.3f, 0.6f, 1f), 12f, 18f, 0.35f);
        if (PlayerTime.Instance != null)
            PlayerTime.Instance.TimeStop(0.06f);
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