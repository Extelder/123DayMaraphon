using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerInputs _inputs;

    [Header("Speed")]
    [SerializeField] private float _walkSpeed;

    [Header("System")]
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Walk()
    {
        _inputs.GetMovingInputs();
        _rigidbody.velocity = transform.rotation * new Vector3(_inputs.MovementHorizontal * _walkSpeed,
        _rigidbody.velocity.y, _inputs.MovementVertical * _walkSpeed);
    }
}
