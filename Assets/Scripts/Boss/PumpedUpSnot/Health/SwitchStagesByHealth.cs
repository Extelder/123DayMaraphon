using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


[Serializable]
public class StageByProcent
{
    [Range(0, 100)] public int Procent;
    public StageStateMachine Stage;
}

public class SwitchStagesByHealth : MonoBehaviour
{
    [SerializeField] private StageByProcent[] _stageByProcents;
    [SerializeField] private Health _health;
    [SerializeField] private StageMachineSwitcher _stageMachineSwitcher;

    private float _procent;

    private void OnEnable()
    {
        _procent = _health.MaxValue / 100;
        _health.HealthValueChanged += OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float value)
    {
        float currentValueInProcent = value / _procent;
        for (int i = 0; i < _stageByProcents.Length; i++)
        {
            if (_stageByProcents[i].Procent <= currentValueInProcent)
            {
                _stageMachineSwitcher.SwitchState(_stageByProcents[i].Stage);
                return;
            }
        }
    }

    private void OnDisable()
    {
        _health.HealthValueChanged -= OnHealthValueChanged;
    }
}