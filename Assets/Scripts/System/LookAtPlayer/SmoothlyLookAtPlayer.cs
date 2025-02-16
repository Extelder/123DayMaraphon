using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class SmoothlyLookAtPlayer : MonoBehaviour
{
    [SerializeField] private float _turnSpeed;

    private Transform _player;

    private void Awake()
    {
        _player = PlayerCharacter.Instance.Transform;
    }

    private void Update()
    {
        Vector3 targetDir = _player.position - transform.position;
        targetDir.y = transform.position.y;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir),
            Time.deltaTime * _turnSpeed);
    }
}