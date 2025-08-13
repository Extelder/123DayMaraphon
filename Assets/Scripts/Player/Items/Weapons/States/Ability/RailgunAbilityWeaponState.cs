using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunAbilityWeaponState : AblityWeaponState
{
    [SerializeField] private AudioSource _chargingSound;
    
    [SerializeField] private MeshRenderer _railgun;
    [SerializeField] private Material _railgunDefaultMaterial;
    [SerializeField] private Material _railgunChargedMaterial;

    [SerializeField] private float _secondsForFullCharge;

    private bool _pressedUp;

    private float _currentSeconds;

    public override void Enter()
    {
        AbilityUsed += OnAbilityUsed;
        CanChanged = false;
        _currentSeconds = 0;
        _chargingSound.Play();
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedUp += OnWeaponAbilityPressedUp;
        _pressedUp = false;
        StopAllCoroutines();
        StartCoroutine(WaitingForPressUp());
        Animator.Ability();
    }

    private void OnAbilityUsed()
    {
        _railgun.material = _railgunDefaultMaterial;
    }

    private IEnumerator WaitingForPressUp()
    {
        while (_pressedUp == false)
        {
            _currentSeconds += 0.02f;
            if (_currentSeconds >= _secondsForFullCharge)
            {
                _railgun.material = _railgunChargedMaterial;
            }

            yield return new WaitForSeconds(0.02f);
        }

        _chargingSound.Stop();
        if (_currentSeconds >= _secondsForFullCharge)
        {
            Animator.SetAnimationTrigger("Shooting");
        }
        else
        {
            Animator.SetAnimationTrigger("Charging");
        }
    }

    public override void Exit()
    {
        AbilityUsed -= OnAbilityUsed;
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedUp -= OnWeaponAbilityPressedUp;
        _railgun.material = _railgunDefaultMaterial;
        base.Exit();
    }

    private void OnDisable()
    {
        AbilityUsed -= OnAbilityUsed;
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedUp -= OnWeaponAbilityPressedUp;
    }

    private void OnWeaponAbilityPressedUp()
    {
        _pressedUp = true;
    }
}