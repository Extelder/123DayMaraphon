using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDEnemyIdleState : State
{
    [SerializeField] private float _minCooldown;
    [SerializeField] private float _maxCooldown;
    [SerializeField] private DoubleDEnemyStateMachine _enemyStateMachine;
    public override void Enter()
    {
        StartCoroutine(WaitToChangeState());
    }

    private IEnumerator WaitToChangeState()
    {
        yield return new WaitForSeconds(Random.Range(_minCooldown, _maxCooldown));
        _enemyStateMachine.Shoot();
    }
    
    public override void Exit()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        Exit();
    }
}
