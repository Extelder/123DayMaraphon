using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SetPlayerPosition : PlayerTrigger
{
    [Inject] private PlayerCharacter _character;
    [SerializeField] private Transform _newPlayerTransform;
    [SerializeField] private Quaternion _newPlayerRotation;
    public override void Triggered()
    {
        _character.Transform.SetPositionAndRotation(_newPlayerTransform.position, _newPlayerRotation);
    }
}
