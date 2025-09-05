using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDEnemyStateMachine : StateMachine
{
    [SerializeField] private State _defaultState;
    [SerializeField] private State _shootState;

    public void Shoot()
    {
        ChangeState(_shootState);
    }

    public void DefaultState()
    {
        ChangeState(_defaultState);
    }
    
    public void Pause()
    {
        ChangeState(_defaultState);
        CurrentState.CanChanged = false;
    }

    public void UnPause()
    {
        CurrentState.CanChanged = true;
        ChangeState(_shootState);
    }
}
