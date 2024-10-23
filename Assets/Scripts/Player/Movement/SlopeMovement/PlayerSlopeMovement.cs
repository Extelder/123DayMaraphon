using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeMovement : MonoBehaviour
{
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _layerMask;
    private RaycastHit _slopeHit;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.useGravity = !OnSlope();
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 
            out _slopeHit, _playerHeight * 0.5f + 0.3f, _layerMask))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }
}
