using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrapable : MonoBehaviour, IGhostTrapable
{

    [SerializeField] private EnemyTrapableStateMachine _stateMachine;
    [SerializeField] private EnemyTrapedState _trapedState;
    private bool _traped;

    [field: SerializeField] public IWeaponVisitor ObjectVisitor { get ; set ; }

    public void Trap()
    {
        if(_traped) 
            return;
        _stateMachine.Trap();
        _traped = true;
    }

    public void UnTrap()
    {
        _traped = false;
        _trapedState.OnUnTraped();
    }
}
