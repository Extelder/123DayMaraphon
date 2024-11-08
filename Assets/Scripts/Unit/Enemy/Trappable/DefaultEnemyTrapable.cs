using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IWeaponVisitor))]
public class DefaultEnemyTrapable : EnemyTrapable
{
    [SerializeField] private EnemyTrapableStateMachine _stateMachine;
    [SerializeField] private EnemyTrapedState _trapedState;
    
    public override void OnTrapped()
    {
        _stateMachine.Trap();
    }

    public override void OnUnTrapped()
    {
        _trapedState.OnUnTraped();
    }
}