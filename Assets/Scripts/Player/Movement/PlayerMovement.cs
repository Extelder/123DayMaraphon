using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private PlayerWalk _walk;

    [SerializeField] private PlayerDash _dash;
    [SerializeField] private PlayerJump _jump;
    [SerializeField] private PlayerDashDown _dashDown;

    [Inject] private PlayerInputs _inputs;

    private bool _canDash = true;
    private bool _canJump = true;

    public void SetCanDash(bool value)
    {
        _canDash = value;
    }

    public void SetCanJump(bool value)
    {
        _canJump = value;
    }

    private void Update()
    {
        _inputs.PlayerMovementInputs.GetMovingInputs();
        _walk.Walk(new Vector3(_inputs.PlayerMovementInputs.MovementHorizontal, 0,
            _inputs.PlayerMovementInputs.MovementVertical));
    }

    private void OnEnable()
    {
        _inputs.PlayerMovementInputs.DashPressedDown += Dashing;
        _inputs.PlayerMovementInputs.DashDownwardsPressedDown += DashingDown;
        _inputs.PlayerMovementInputs.JumpPressedDown += Jumping;
    }

    private void OnDisable()
    {
        _inputs.PlayerMovementInputs.DashPressedDown -= Dashing;
        _inputs.PlayerMovementInputs.DashDownwardsPressedDown -= DashingDown;
        _inputs.PlayerMovementInputs.JumpPressedDown -= Jumping;
    }

    private void Dashing()
    {
        if (_canDash)
            _dash.Dash();
    }

    private void DashingDown()
    {
        _dashDown.DashDown();
    }

    private void Jumping()
    {
        if (_canJump)
            _jump.Jump();
    }
}