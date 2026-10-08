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
    [SerializeField] private float _burstDamageMultiplier = 2f;

    private RaycastWeaponShoot _weaponShoot;
    private bool _pressedUp;

    private float _currentSeconds;

    private void Awake()
    {
        _weaponShoot = GetComponent<RaycastWeaponShoot>();
    }

    public override void Enter()
    {
        SetBursting(false);
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
        // Короткое нажатие — шар (клип RailgunAbilityBoom по триггеру "Shooting"),
        // полный заряд — берст (клип RailgunAbilityShooting по триггеру "Charging").
        if (_currentSeconds >= _secondsForFullCharge)
        {
            SetBursting(true);
            Animator.SetAnimationTrigger("Charging");
        }
        else
        {
            Animator.SetAnimationTrigger("Shooting");
        }
    }

    private void SetBursting(bool bursting)
    {
        if (_weaponShoot == null)
            return;

        _weaponShoot.Bursting = bursting;
        _weaponShoot.BurstDamageMultiplier = _burstDamageMultiplier;
    }

    public override void Exit()
    {
        AbilityUsed -= OnAbilityUsed;
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedUp -= OnWeaponAbilityPressedUp;
        _railgun.material = _railgunDefaultMaterial;
        SetBursting(false);
        base.Exit();
    }

    private void OnDisable()
    {
        SetBursting(false);
        AbilityUsed -= OnAbilityUsed;
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedUp -= OnWeaponAbilityPressedUp;
    }

    private void OnWeaponAbilityPressedUp()
    {
        _pressedUp = true;
    }
}