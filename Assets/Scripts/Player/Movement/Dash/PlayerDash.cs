using UniRx;
using UnityEngine;
using Zenject;

public class PlayerDash : Dashing
{
    [Header("Settings")] 
    [SerializeField] private float _dashCooldown;
    [Inject] private PlayerInputs _playerInputs;

    private CompositeDisposable _dashDisposable = new CompositeDisposable();

    public void Dash()
    {
        if (!cooldownRecovered)
            return;

        Vector3 inputDirection = new Vector3(_playerInputs.MovementHorizontal, 0, _playerInputs.MovementVertical);

        if (inputDirection == Vector3.zero)
            inputDirection = orientation.forward;

        Vector3 direction = orientation.TransformDirection(inputDirection);

        Vector3 forceToApply = (direction * dashSpeed + orientation.up * dashUpwardForce);
        float cooldown = _dashCooldown;

        AddImpulse(forceToApply, cooldown, _dashDisposable);
    }

    private void OnDisable()
    {
        _dashDisposable.Clear();
    }
}