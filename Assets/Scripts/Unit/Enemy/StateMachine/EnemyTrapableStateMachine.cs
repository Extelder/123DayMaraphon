using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrapableStateMachine : EnemyStateMachine
{
    [SerializeField] private State _trapState;

    public void Trap()
    {
        CurrentState.CanChanged = true;
        ChangeState(_trapState);
    }
}
