using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerFallTrigger : PlayerTrigger
{
    [Inject] private PlayerCharacter _character;
    [Inject] private PlayerHealth _playerHealth;

    [SerializeField] private float _damage = 10;

    public override void Triggered()
    {
        _character.Transform.position = _character.PlayerLevelCheckPoint.LastCheckPoint;
        _playerHealth.TakeDamage(_damage);
    }
}