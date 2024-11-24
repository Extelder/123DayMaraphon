using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MovementSpeedLerping
{
    [SerializeField] private bool _resetRigidBodyYAfterJump;

    public event Action Jumped;

    private Rigidbody _rigidbody;

    private bool _canJump;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        if (!_canJump)
            return;
        StartCoroutine(SmoothlyLerpMoveSpeed());

        if (_resetRigidBodyYAfterJump)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        _rigidbody.AddForce(Vector3.up * moveSpeed, ForceMode.Impulse);
        Jumped?.Invoke();
    }

    public void Jump(Vector3 velocity, bool resetRigidbodyYAfterJump = true)
    {
        if (resetRigidbodyYAfterJump)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
    }

    public void EnableJump()
    {
        _canJump = true;
    }

    public void DisableJump()
    {
        _canJump = false;
    }
}