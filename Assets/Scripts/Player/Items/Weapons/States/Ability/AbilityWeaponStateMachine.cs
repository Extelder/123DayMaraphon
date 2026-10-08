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
        if (!Item.TakeUpped || !_weaponAbility.Filled.Value)
            return;

        // После абилки машина остаётся в её состоянии, а ChangeState в то же состояние ничего не делает —
        // повторно абилка срабатывала только после свапа. Выходим в стартовое состояние и входим заново.
        if (CurrentState == _ablityState)
        {
            if (!_ablityState.CanChanged)
                return;
            DefaultState();
        }

        CurrentState.CanChanged = true;
        ChangeState(_ablityState);
    }
}