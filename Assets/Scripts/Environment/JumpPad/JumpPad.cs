using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private Vector3 _velocity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerJump> (out PlayerJump playerJump))
        {
            playerJump.Jump(_velocity);
        }
    }
}
