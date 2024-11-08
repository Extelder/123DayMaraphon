using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField] public Transform Transform { get; private set; }
    public Vector3 PlayerPositionForNavMesh { get; private set; }
    [SerializeField] private float _rayRange;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Vector3 _offset;

    private RaycastHit _hit;

    private void Update()
    {

        if (Physics.Raycast(Transform.position, -Transform.up, out _hit, _rayRange, _layer))
        {
            if (_hit.collider.TryGetComponent<Ground>(out Ground ground))
            {
                PlayerPositionForNavMesh = _hit.point + _offset;
                return;
            }
        }
    }

}