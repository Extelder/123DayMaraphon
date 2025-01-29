using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    public bool Detected { get; private set; }

    public event Action GroundDetected;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == _player)
            return;
        GroundDetected?.Invoke();
        Detected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
            return;
        Detected = false;
    }
}