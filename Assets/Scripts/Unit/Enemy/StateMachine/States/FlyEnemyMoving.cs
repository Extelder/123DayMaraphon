using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMoving : State
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FlyEnemyStateMachine _stateMachine;
    [SerializeField] private float _randomImpulse;

    public override void Enter()
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _stateMachine.Shoot();
    }

    private Vector3 force => new Vector3(
        Random.Range(-_randomImpulse, _randomImpulse),
        Random.Range(-_randomImpulse, _randomImpulse),
        Random.Range(-_randomImpulse, _randomImpulse));
}