using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _startState;
    public State CurrentState { get; private set; }

    public virtual void Start()
    {
        CurrentState = _startState;
        CurrentState.Enter();
    }

    public void ChangeState(State state)
    {
        if (CurrentState.CanChanged && CurrentState != state)
        {
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}