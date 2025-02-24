using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUntrappedToDefaultState : MonoBehaviour
{
    [SerializeField] private DefaultEnemyTrapable _defaultEnemyTrapable;
    [SerializeField] private StateMachine _machine;

    private void OnEnable()
    {
        _defaultEnemyTrapable.UnTrapped += GoToDefaultState;
    }

    private void GoToDefaultState()
    {
        _machine.DefaultState();
    }

    private void OnDisable()
    {
        _defaultEnemyTrapable.UnTrapped -= GoToDefaultState;
    }
}
