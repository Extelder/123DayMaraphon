using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RailgunAbility : WeaponAbility
{
    [SerializeField] private AudioSource _spawnAudio;

    [Inject] private Pools _pools;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _spawnPoint;

    public void PerformSpawnLightningBall()
    {
        _spawnAudio.Play();
        Projectile projectile = _pools.LightingProjectilePool.GetFreeElement(_spawnPoint.position, _camera.rotation)
            .GetComponent<Projectile>();
        projectile.Initiate(_camera.position + _camera.forward * 150, false);
    }

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
    }
}