using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [SerializeField] private OverlapSettings _overlapSettings;

    public void CheckForWall(out Collider[] colliders, out int size)
    {
        colliders = _overlapSettings.Colliders;
        size = _overlapSettings.Size;
        OverlapSphere();
    }

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius);
    }
}