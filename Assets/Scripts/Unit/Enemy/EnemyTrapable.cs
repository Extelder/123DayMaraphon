using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IWeaponVisitor))]
public class EnemyTrapable : MonoBehaviour, IGhostTrapable
{
    [SerializeField] private EnemyTrapableStateMachine _stateMachine;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EnemyTrapedState _trapedState;
    [SerializeField] private Vector3 _lineStartPointOffset;

    private bool _traped;
    private Ghost _ghost;

    public IWeaponVisitor ObjectVisitor { get; set; }

    private void Awake()
    {
        ObjectVisitor = GetComponent<IWeaponVisitor>();
        _lineRenderer.useWorldSpace = true;
    }

    public void Trap(Ghost ghost)
    {
        if (_traped)
            return;
        _lineRenderer.SetPosition(0, transform.position + _lineStartPointOffset);
        _lineRenderer.SetPosition(1, ghost.transform.position);
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