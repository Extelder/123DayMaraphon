using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RailgunAbility : WeaponAbility
{
    public override void OnAbilityUsed()
    {
        base.OnAbilityUsed();
        CameraShakeInvoke();
    }
}
