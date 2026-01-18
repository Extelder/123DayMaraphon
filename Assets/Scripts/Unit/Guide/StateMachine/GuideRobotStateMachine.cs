using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRobotStateMachine : StateMachine
{
    [SerializeField] private State _move;
    [SerializeField] private State idle;
    [SerializeField] private UnitSpeakState _speak;
    
    public void Move()
    {
        ChangeState(_move);
    }

    public void Speak(string textToPrint, Transform targetPoint)
    {
        _speak.SetParametrs(textToPrint, targetPoint);
        ChangeState(_speak);
    }

    public void Idle()
    {
        ChangeState(idle);
    }
}
