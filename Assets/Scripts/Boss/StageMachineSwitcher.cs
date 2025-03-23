using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMachineSwitcher : MonoBehaviour
{
    [SerializeField] private StageStateMachine[] _stages;

    [SerializeField] private bool _startMachineOnBegin;
    private int _currentStageIndex = 0;

    private StageStateMachine _currentStage;

    public void Start()
    {
        if (_startMachineOnBegin)
            StartSwitchMachine();
    }

    public void StartSwitchMachine()
    {
        _currentStage = _stages[_currentStageIndex];
        _currentStage.StartMachine();
    }

    public void SwitchState(StageStateMachine stageStateMachine)
    {
        if (_currentStage == stageStateMachine)
            return;
        _currentStage?.StopMachine();
        _currentStage = stageStateMachine;
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