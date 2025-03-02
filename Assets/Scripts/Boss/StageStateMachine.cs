using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageStateMachine : StateMachine
{
    public abstract void StartMachine();

    public virtual void StopMachine()
    {
        
    }
}
