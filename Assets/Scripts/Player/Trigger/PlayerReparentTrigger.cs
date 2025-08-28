using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerReparentTrigger : PlayerTrigger
{
    [SerializeField] private Transform _newParent;

    [Inject] private PlayerCharacter _playerCharacter;

    public override void OnTriggered()
    {
        _playerCharacter.Transform.parent = _newParent;
        _playerCharacter.Transform.SetParent(_newParent);
    }
}