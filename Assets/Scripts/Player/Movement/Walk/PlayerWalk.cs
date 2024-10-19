using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MonoBehaviour
{
    [Header("Speed")] [SerializeField] private float _walkSpeed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Walk(Vector3 input)
    {
        _rigidbody.velocity = transform.rotation * new Vector3(input.x * _walkSpeed,
            _rigidbody.velocity.y, input.z * _walkSpeed);
    }
}