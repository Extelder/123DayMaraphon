using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnMovingObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerStepOnMovingObject>(out PlayerStepOnMovingObject playerStepOnMovingObject))
        {
            playerStepOnMovingObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerStepOnMovingObject>(out PlayerStepOnMovingObject playerStepOnMovingObject))
        {
            playerStepOnMovingObject.transform.parent = null;
        }
    }
}