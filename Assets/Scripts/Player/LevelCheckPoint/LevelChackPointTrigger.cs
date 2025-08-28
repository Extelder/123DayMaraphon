using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelChackPointTrigger : PlayerTrigger
{
    [Inject] private PlayerCharacter _playerCharacter;

    public override void OnTriggered()
    {
        _playerCharacter.PlayerLevelCheckPoint.SetNewCheckPoint(transform.position);
    }
}