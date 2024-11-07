using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappableFlyEnemyStateMachine : FlyEnemyStateMachine
{
    [SerializeField] private State _trapState;

    public void Trap()
    {
        CurrentState.CanChanged = true;
        ChangeState(_trapState);
    }
}
