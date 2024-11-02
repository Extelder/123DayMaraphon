using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private State _move;
    [SerializeField] private State _attack;

    public void Attack()
    {
        ChangeState(_attack);
    }

    public void Move()
    {
        ChangeState(_move);
    }
}
