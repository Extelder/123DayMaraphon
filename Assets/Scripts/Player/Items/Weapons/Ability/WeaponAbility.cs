using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour
{
    [SerializeField] private AblityWeaponState _weaponAbilityState;

    public event Action AbilityPerformed;
    public event Action CameraShake;

    private void OnEnable()
    {
        _weaponAbilityState.AbilityUsed += OnAbilityUsed;
    }

    private void OnDisable()
    {
        _weaponAbilityState.AbilityUsed -= OnAbilityUsed;
        OnDisableVirtual();
    }

    protected virtual void OnDisableVirtual()
    {
    }

    public virtual void OnAbilityUsed()
    {
        AbilityPerformed?.Invoke();
    }

    public void CameraShakeInvoke()
    {
        CameraShake?.Invoke();
    }
}
