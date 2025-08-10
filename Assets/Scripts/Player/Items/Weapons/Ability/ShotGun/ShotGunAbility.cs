using UnityEngine;
using Zenject;

public class ShotGunAbility : WeaponAbility
{
    [SerializeField] private Transform _ghostSpawnPoint;
    [SerializeField] private Transform _camera;

    [Inject] private Pools _pools;

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
        _pools.GhostPool.GetFreeElement(_ghostSpawnPoint.position, _camera.transform.rotation);
    }
}