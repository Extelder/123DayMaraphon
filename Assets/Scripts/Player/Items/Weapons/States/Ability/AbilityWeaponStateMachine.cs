using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWeaponStateMachine : WeaponStateMachine
{
    [SerializeField] private State _ablityState;

    public override void OnEnable()
    {
        base.OnEnable();
        PlayerInputs.WeaponAbilityPressedDown += OnAbilityPressedDown;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerInputs.WeaponAbilityPressedDown -= OnAbilityPressedDown;
    }

    private void OnAbilityPressedDown()
    {
        if (Item.TakeUpped)
        {
            CurrentState.CanChanged = true;
            ChangeState(_ablityState);
        }
    }
}