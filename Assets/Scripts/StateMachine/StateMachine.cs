using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _startState;
    public State CurrentState { get; private set; }

    public void DefaultState()
    {
        ChangeState(_startState);
    }

    public virtual void OnEnable()
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