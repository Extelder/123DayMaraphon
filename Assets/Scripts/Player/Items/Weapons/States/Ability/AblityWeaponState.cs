using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AblityWeaponState : State
{
    [Inject] private PlayerInputs _playerInputs;
    [SerializeField] private AbilityWeaponAnimator _animator;

    public event Action AbilityUsed;

    public override void Enter()
    {
        CanChanged = false;
        _animator.Ability();
    }

    public void PerformAbility()
    {
        AbilityUsed?.Invoke();
    }

    public void AnimationEnd()
    {
        CanChanged = true;
    }
}