using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : OpenCloseAnimator
{
    [SerializeField] private Door _door;

    private void OnEnable()
    {
        _door.Opened += OpenAnimation;
        _door.Closed += CloseAnimation;
    }

    private void OnDisable()
    {
        _door.Opened -= OpenAnimation;
        _door.Closed -= CloseAnimation;
    }
}