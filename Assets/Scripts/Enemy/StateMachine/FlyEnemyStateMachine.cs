using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyStateMachine : StateMachine
{
    [SerializeField] private State _shootingState;
    [SerializeField] private State _defaultState;

    public void DefaultState()
    {
        ChangeState(_defaultState);
    }

    public void Shoot()
    {
        ChangeState(_shootingState);
    }
}