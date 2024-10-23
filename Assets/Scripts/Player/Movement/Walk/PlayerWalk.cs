using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MonoBehaviour
{
    [field: Header("Speed")]
    [field: SerializeField] public float WalkSpeed;

    [SerializeField] private PlayerMovement _playerMovement;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Walk(Vector3 input)
    {
        _rigidbody.velocity = transform.rotation * new Vector3(input.x * WalkSpeed,
            _rigidbody.velocity.y, input.z * WalkSpeed);
    }
}