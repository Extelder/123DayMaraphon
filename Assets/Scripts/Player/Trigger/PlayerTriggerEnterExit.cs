using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTriggerEnterExit : PlayerTrigger
{
    public override void Triggered()
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
