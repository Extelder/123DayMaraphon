using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerDash : Dashing
{
    [Header("Settings")] [SerializeField] private float _dashCooldown;
    [Inject] private PlayerInputs _playerInputs;

    private CompositeDisposable _dashDisposable = new CompositeDisposable();

    private bool _canDash;

    public event Action Dashed;

    public void Dash()
    {
        if (!_canDash)
            return;

        if (!cooldownRecovered)
            return;

        Dashed?.Invoke();

        Vector3 inputDirection = new Vector3(_playerInputs.PlayerMovementInputs.MovementHorizontal, 0, _playerInputs.PlayerMovementInputs.MovementVertical);

        Vector3 direction = orientation.TransformDirection(inputDirection);

        if (inputDirection == Vector3.zero)
        {
            inputDirection = orientation.forward;
            direction = inputDirection;
        }


        Vector3 forceToApply = (direction * dashSpeed + orientation.up * dashUpwardForce);
        float cooldown = _dashCooldown;

        AddImpulse(forceToApply, cooldown, _dashDisposable);
    }

    public void DisableDash()
    {
        _canDash = false;
    }

    public void EnableDash()
    {
        _canDash = true;
    }

    private void OnDisable()
    {
        _dashDisposable.Clear();
    }
}