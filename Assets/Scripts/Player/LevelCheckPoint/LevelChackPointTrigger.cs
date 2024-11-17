using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelChackPointTrigger : PlayerTrigger
{
    [Inject] private PlayerCharacter _playerCharacter;

    public override void Triggered()
    {
        _playerCharacter.PlayerLevelCheckPoint.SetNewCheckPoint(transform.position);
    }
}