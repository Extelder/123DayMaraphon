using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class GroundChecker : MonoBehaviour
{
    public bool Detected { get; private set; }

    public event Action GroundDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Ground>(out Ground ground))
        {
            GroundDetected?.Invoke();
            Detected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Ground>(out Ground ground))
        {
            Detected = false;
        }
    }
}