using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWeaponAnimator : WeaponAnimator
{
    [SerializeField] private string _abilityAnimationTriggerName = "Ability";

    public void Ability()
    {
        SetAnimationTrigger(_abilityAnimationTriggerName);
    }
}