using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Door, IInteractable
{
    public void Interact()
    {
        TryOpenClose();
    }
}