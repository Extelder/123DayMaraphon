using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTrappable : EnemyTrapable
{
    [SerializeField] private TurretEnemyStateMachine _enemyStateMachine;
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
