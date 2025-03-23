using UnityEngine;
using System;

public class OverlapSettings : MonoBehaviour
{
    public Collider[] Colliders = new Collider[10];
    public int Size;
     public float _sphereRadius;

    public LayerMask _searchLayer;
    public Transform _overlapPoint;
    public Vector3 _boxSize;
    public Vector3 _positionOffset;

    public void SetOverlapPoint(Transform newOverlapPoint)
    {
        _overlapPoint = newOverlapPoint;
    }

    public void SetSearchMask(LayerMask newMask)
    {
        _searchLayer = newMask;
    }

    public void SetOffset(Vector3 offset)
    {
        _positionOffset = offset;
    }

    public void SetBoxSize(Vector3 size)
    {
        _boxSize = size;
    }

    public void SetSphereRadius(float radius)
    {
        if (radius < 0f)
            throw new ArgumentOutOfRangeException(nameof(radius));

        _sphereRadius = radius;
    }



}
