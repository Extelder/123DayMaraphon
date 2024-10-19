using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MonoBehaviour
{
    [Header("Speed")][SerializeField] private float _walkSpeed;

    [SerializeField] private PlayerMovement _playerMovement;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Walk(Vector3 input)
    {
        _playerMovement.CurrentMoveSpeed = _walkSpeed;
        _rigidbody.velocity = transform.rotation * new Vector3(input.x * _playerMovement.CurrentMoveSpeed,
            _rigidbody.velocity.y, input.z * _playerMovement.CurrentMoveSpeed);
    }
}