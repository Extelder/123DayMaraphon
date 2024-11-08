using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyTrappable : EnemyTrapable
{
    [SerializeField] private FlyEnemyStateMachine _enemyStateMachine;
    [SerializeField] private LookAtPlayer _lookAtPlayer;

    public override void OnTrapped()
    {
        _enemyStateMachine.Pause();
        _lookAtPlayer.enabled = false;
    }

    public override void OnUnTrapped()
    {
        _enemyStateMachine.UnPause();
        _lookAtPlayer.enabled = true;
    }
}