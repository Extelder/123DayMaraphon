using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerJumpSound : MonoBehaviour
{
    [SerializeField] private PlayerJump _jump;
    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _jump.Jumped += OnJumped;
    }

    private void OnJumped()
    {
        _pools.JumpSoundPool.GetFreeElement(transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        _jump.Jumped -= OnJumped;
    }
}