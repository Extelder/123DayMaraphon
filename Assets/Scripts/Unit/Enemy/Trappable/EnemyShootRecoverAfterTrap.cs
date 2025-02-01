using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootRecoverAfterTrap : MonoBehaviour
{
    [SerializeField] private DefaultEnemyTrapable _enemyTrapable;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    
    private void OnEnable()
    {
        _enemyTrapable.UnTrapped += OnUntrapped;
    }

    private void OnUntrapped()
    {
        _enemyStateMachine.Move();
    }
    
    private void OnDisable()
    {
        _enemyTrapable.UnTrapped -= OnUntrapped;
    }
}
