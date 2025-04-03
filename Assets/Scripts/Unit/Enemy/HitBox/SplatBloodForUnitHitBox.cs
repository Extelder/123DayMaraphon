using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
public class SplatBloodForUnitHitBox : MonoBehaviour
{
    [Inject] private Pools _pools;
    [SerializeField] private float _rayRange;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _randomRangeMultiplier;
    [SerializeField] private UnitHitBox _hitBox;
    [SerializeField] private int _bloodSplatterPerHit;
    [SerializeField] private float _spreadFactor;

    private Vector3 _currentRaycastOffset;
    private RaycastHit _hit;

    private void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _pools.BloodSplatPool.GetFreeElement(spawnPoint, Quaternion.LookRotation(_hit.normal));
        currentObject.transform.localScale = ChangeScale();
    }

    private void SplatBlood()
    {
        _currentRaycastOffset = Random.insideUnitSphere * _randomRangeMultiplier;
        if (Physics.Raycast(transform.position, _currentRaycastOffset, out _hit, _rayRange, _layer))
        {
            var hitCollider = _hit.collider;
            if (hitCollider.TryGetComponent<BloodSplatableObject>(out BloodSplatableObject bloodSplatable))
            {
                SpawningDecal(_hit.point);
            }
        }
    }


    private Vector3 ChangeScale()
    {
        return new Vector3
        {
            x = Random.Range(1, _spreadFactor),
            y = Random.Range(1, _spreadFactor),
            z = 1
        };
    }

    private void OnEnable()
    {
        _hitBox.Hit += OnHitted;
    }

    private void OnDisable()
    {
        _hitBox.Hit -= OnHitted;
    }

    private void OnHitted()
    {
        for (int i = 0; i < _bloodSplatterPerHit; i++)
        {
            SplatBlood();
        }
    }


}
