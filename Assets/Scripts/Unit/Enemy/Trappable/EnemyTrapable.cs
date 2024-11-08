using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrapable : MonoBehaviour, IGhostTrapable
{
    [SerializeField] private LineRenderer _lineRenderer;
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
        _traped = true;
        OnTrapped();
    }

    public void UnTrap()
    {
        _traped = false;
        _lineRenderer.SetPosition(1, transform.position + _lineStartPointOffset);
        OnUnTrapped();
    }

    public virtual void OnTrapped()
    {
    }

    public virtual void OnUnTrapped()
    {
    }

    private void OnDisable()
    {
        if (!_ghost)
            return;
        if (_ghost.TrapedUnits.Contains(this))
            _ghost.TrapedUnits.Remove(this);
    }
    
}
