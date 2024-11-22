using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPosition : PlayerTrigger
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _newPlayerTransform;
    [SerializeField] private Quaternion _newPlayerRotation;
    public override void Triggered()
    {
        _playerTransform.SetPositionAndRotation(_newPlayerTransform.position, _newPlayerRotation);
    }
}
