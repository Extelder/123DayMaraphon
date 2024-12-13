using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TranslatePlayer : MonoBehaviour
{
    [Inject] private PlayerCharacter _character;
    [SerializeField] private float _playerMovement;
    [SerializeField] private float _playerMovementMultiplier;
    private void Update()
    {
        transform.Translate(Vector3.forward * _playerMovement);
        _playerMovement *= _playerMovementMultiplier;
    }
}
