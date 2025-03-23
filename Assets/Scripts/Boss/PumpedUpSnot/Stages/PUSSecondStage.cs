using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSSecondStage : StageStateMachine
{
    [SerializeField] private State[] _states;
    [SerializeField] private float _maxRandomToChangeState;
    [SerializeField] private float _minRandomToChangeState;

    public override void StartMachine()
    {
        CurrentState.Enter();
        StartCoroutine(RandomlyChangeStates());
    }

    public override void StopMachine()
    {
        StopAllCoroutines();
    }

    private IEnumerator RandomlyChangeStates()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minRandomToChangeState, _maxRandomToChangeState));
            ChangeState(_states[Random.Range(0, _states.Length)]);
        }
    }
}