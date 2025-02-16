using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSmoothlyLookAtPlayer : MonoBehaviour
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir),
            Time.deltaTime * _turnSpeed);
    }
}
