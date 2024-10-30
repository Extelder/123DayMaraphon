using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpForceByJumpsMultiplayer;
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

        if (_resetRigidBodyYAfterJump)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        Jumped?.Invoke();
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