using UnityEngine;
using Zenject;

public class ShotGunAbility : WeaponAbility
{
    [SerializeField] private Transform _ghostSpawnPoint;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _extraSpawnDistance = 5f;
    [SerializeField] private float _wallPadding = 1f;

    [Inject] private Pools _pools;

    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();

        // Призрак встаёт дальше по взгляду (но не в стену) и разворачивается лицом к камере.
        Vector3 forward = _camera.forward;
        Vector3 position = _ghostSpawnPoint.position;
        float distance = _extraSpawnDistance;
        if (Physics.Raycast(position, forward, out RaycastHit hit, distance + _wallPadding,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            distance = Mathf.Max(0f, hit.distance - _wallPadding);

        Quaternion facingCamera = _camera.rotation * Quaternion.Euler(0f, 180f, 0f);
        _pools.GhostPool.GetFreeElement(position + forward * distance, facingCamera);
    }
}