using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyStateMachine : StateMachine
{
    [SerializeField] private State _shootingState;
    [SerializeField] private State _trapState;

    public void DefaultState()
    {
        ChangeState(_shootingState);
    }

    public void Shoot()
    {
        ChangeState(_shootingState);
    }

    public void Pause()
    {
        ChangeState(_trapState);
        CurrentState.CanChanged = false;
    }

    public void UnPause()
    {
        CurrentState.CanChanged = true;
        ChangeState(_shootingState);
    }
}