using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyTrappable : EnemyTrapable
{
    [SerializeField] private FlyEnemyStateMachine _enemyStateMachine;

    public override void OnTrapped()
    {
        _enemyStateMachine.Pause();
    }

    public override void OnUnTrapped()
    {
        _enemyStateMachine.UnPause();
    }
}