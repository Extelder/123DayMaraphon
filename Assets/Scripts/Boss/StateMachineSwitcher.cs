using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSwitcher : MonoBehaviour
{
    [SerializeField] private StageStateMachine[] _stages;

    private int _currentStageIndex = 0;

    private StageStateMachine _currentStage;

    public void Start()
    {
        _currentStage = _stages[_currentStageIndex];
        _currentStage.StartMachine();
    }

    public void SwitchToNextStage()
    {
        _currentStage?.StopMachine();
        _currentStageIndex += 1;
        _currentStage = _stages[_currentStageIndex];
        _currentStage.StartMachine();
    }
}