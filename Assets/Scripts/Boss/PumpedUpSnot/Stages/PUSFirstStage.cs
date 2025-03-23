using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PUSFirstStage : StageStateMachine
{
    [SerializeField] private State[] _states;
    [SerializeField] private float _maxRandomToChangeState;
    [SerializeField] private float _minRandomToChangeState;

    public override void StartMachine()
    {
        StartCoroutine(RandomlyChangeStates());
    }

    public override void StopMachine()
    {
        StopAllCoroutines();

        CurrentState.CanChanged = true;
        CurrentState.Exit();
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