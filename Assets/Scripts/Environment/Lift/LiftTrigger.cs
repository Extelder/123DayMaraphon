using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : PlayerTrigger
{
    [SerializeField] private Transform _lift;
    [SerializeField] private Transform _tpPoint;

    public override void OnTriggered()
    {
        _lift.position = _tpPoint.position;
    }
}