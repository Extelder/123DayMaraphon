using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartlerpPlayerTrigger : PlayerTrigger
{
    [SerializeField] private LerpInPoint[] _lerps;

    public override void Triggered()
    {
        for (int i = 0; i < _lerps.Length; i++)
        {
            _lerps[i].Lerp();
        }
    }
}