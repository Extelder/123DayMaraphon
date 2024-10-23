using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using Zenject;

public class PlayerSlopeMovement : MonoBehaviour
{
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _playerHeight;
    [SerializeField] private PlayerWalk _playerWalk;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _forceToGround;
    [SerializeField] private LayerMask _layerMask;
    [Inject] private PlayerInputs _playerInputs;
    private RaycastHit _slopeHit;

    private Rigidbody _rigidbody;

    public event Action PlayerSloped;

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
