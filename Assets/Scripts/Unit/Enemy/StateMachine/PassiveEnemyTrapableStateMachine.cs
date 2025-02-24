using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyTrapableStateMachine : PassiveEnemyStateMachine
{
    [SerializeField] private State _trapState;

    public void Trap()
    {
        CurrentState.CanChanged = true;
        ChangeState(_trapState);
    }
}
