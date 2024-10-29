using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [Header("Settings")] public int _jumps;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpForceByJumpsMultiplayer;
    [SerializeField] private bool _resetRigidBodyYAfterJump;

    [SerializeField] private GroundChecker _groundChecker;

    public event Action Jumped;

    private Rigidbody _rigidbody;

    private bool _canJump;
    private int _jumpsAvailable;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _groundChecker.GroundDetected += OnGroundDetected;
    }

    private void OnDisable()
    {
        _groundChecker.GroundDetected -= OnGroundDetected;
    }

    private void OnGroundDetected()
    {
        _jumpsAvailable = _jumps;
    }

    public void Jump()
    {
        if (_jumpsAvailable <= 0)
            return;
        if (!_canJump)
            return;

        if (_resetRigidBodyYAfterJump)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        _jumpsAvailable--;

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