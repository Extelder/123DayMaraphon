using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJumpDashZoneTrigger : MonoBehaviour
{
    private PlayerMovement _movement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            _movement = playerMovement;
            playerMovement.SetCanDash(false);
            playerMovement.SetCanJump(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.SetCanDash(true);
            playerMovement.SetCanJump(true);
        }
    }

    private void OnDestroy()
    {
        _movement?.SetCanDash(true);
        _movement?.SetCanJump(true);
    }
}