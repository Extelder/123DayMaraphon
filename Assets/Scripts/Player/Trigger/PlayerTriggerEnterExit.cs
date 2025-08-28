using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTriggerEnterExit : PlayerTrigger
{
    public override void OnTriggered()
    {
        
    }

    public abstract void TriggeredExit();

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            TriggeredExit();
        }
    }
}
