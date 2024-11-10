using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MovementSpeedLerping
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Walk(Vector3 input)
    {
        StartCoroutine(SmoothlyLerpMoveSpeed());
        _rigidbody.velocity = transform.rotation * new Vector3(input.x * moveSpeed,
            _rigidbody.velocity.y, input.z * moveSpeed);
    }
}