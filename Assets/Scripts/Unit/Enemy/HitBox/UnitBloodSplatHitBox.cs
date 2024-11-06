using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
public class UnitBloodSplatHitBox : MonoBehaviour, IWeaponVisitor
{
    [Inject] private Pools _pools;
    [SerializeField] private float _rayRange;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _randomRangeMultiplier;

    private Vector3 _currentRaycastOffset;
    private RaycastHit _hit;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        SplatBlood();
    }

    public void Visit(Projectile projectile)
    {
        SplatBlood();
    }

    public void Visit(Ghost ghost, float damage)
    {
        SplatBlood();
    }

    private void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _pools.BloodSplatPool.GetFreeElement(spawnPoint, Quaternion.identity);
    }

    private void SplatBlood()
    {
        _currentRaycastOffset = Random.insideUnitCircle * _randomRangeMultiplier;
        if (Physics.Raycast(transform.position, _currentRaycastOffset, out _hit, _rayRange, _layer))
        {
            var hitCollider = _hit.collider;
            if (hitCollider.TryGetComponent<Ground>(out Ground ground))
            {
                Debug.Log(hitCollider.gameObject.name);
                SpawningDecal(_currentRaycastOffset);
            }
        }
    }
}
