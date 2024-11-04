using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShotGunAbility : WeaponAbility
{
    [SerializeField] private Transform _ghostSpawnPoint;

    [Inject] private Pools _pools;

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        _pools.GhostPool.GetFreeElement(_ghostSpawnPoint.position);
    }
}