using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWeaponStateMachine : WeaponStateMachine
{
    [SerializeField] private State _ablityState;

    [SerializeField] private WeaponAbilityAmount _weaponAbility;

    public override void OnEnable()
    {
        base.OnEnable();
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedDown += OnAbilityPressedDown;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerInputs.PlayerWeaponInputs.WeaponAbilityPressedDown -= OnAbilityPressedDown;
    }

    private void OnAbilityPressedDown()
    {
        if (Item.TakeUpped && _weaponAbility.Filled.Value)
        {
            CurrentState.CanChanged = true;
            ChangeState(_ablityState);
        }
    }
}