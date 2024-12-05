using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractPlayerTrigger : PlayerTriggerEnterExit
{
    [SerializeField] private Door _door;

    public override void Triggered()
    {
        _door.TryOpenClose();
    }

    public override void TriggeredExit()
    {
        _door.TryOpenClose();
    }
}