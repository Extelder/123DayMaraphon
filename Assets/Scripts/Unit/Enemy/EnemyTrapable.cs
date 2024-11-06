using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IWeaponVisitor))]
public class EnemyTrapable : MonoBehaviour, IGhostTrapable
{
    [SerializeField] private EnemyTrapableStateMachine _stateMachine;
    [SerializeField] private EnemyTrapedState _trapedState;

    private bool _traped;
    private Ghost _ghost;

    public IWeaponVisitor ObjectVisitor { get; set; }

    private void Awake()
    {
        ObjectVisitor = GetComponent<IWeaponVisitor>();
    }

    public void Trap(Ghost ghost)
    {
        if (_traped)
            return;
        _ghost = ghost;
        _stateMachine.Trap();
        _traped = true;
    }

    public void UnTrap()
    {
        _traped = false;
        _trapedState.OnUnTraped();
    }

    private void OnDisable()
    {
        if (!_ghost)
            return;
        if (_ghost.TrapedUnits.Contains(this))
            _ghost.TrapedUnits.Remove(this);
    }
}