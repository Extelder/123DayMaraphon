using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundChecker : MonoBehaviour
{
    public bool Detected { get; private set; }

    public event Action GroundDetected;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<Ground>(out Ground ground) || other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            Detected = true;
        }
    }
}