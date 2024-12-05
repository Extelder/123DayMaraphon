using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnMovingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStepOnMovingObject>(
            out PlayerStepOnMovingObject playerStepOnMovingObject))
        {
            playerStepOnMovingObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStepOnMovingObject>(
            out PlayerStepOnMovingObject playerStepOnMovingObject))
        {
            playerStepOnMovingObject.transform.parent = null;
        }
    }
}