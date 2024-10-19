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
    [SerializeField] private PlayerDashDown _dashDown;
    [SerializeField] private PlayerJump _jump;
    
    [Inject] private PlayerInputs _inputs;

    public bool Moving { get; private set; }


    private void Update()
    {
        _inputs.GetMovingInputs();

        _walk.Walk(new Vector3(_inputs.MovementHorizontal, 0, _inputs.MovementVertical));
    }

    private void OnEnable()
    {
        _inputs.DashPressed += Dashing;
        _inputs.DashDownPressed += DashingDown;
        _inputs.JumpPressed += Jumping;
    }

    private void OnDisable()
    {
        _inputs.DashPressed -= Dashing;
        _inputs.DashDownPressed -= DashingDown;
        _inputs.JumpPressed -= Jumping;
    }

    private void Dashing()
    {
        _dash.Dash();
    }
    
    private void DashingDown()
    {
        _dashDown.DashDown();
    }

    private void Jumping()
    {
        _jump.Jump();
    }
}