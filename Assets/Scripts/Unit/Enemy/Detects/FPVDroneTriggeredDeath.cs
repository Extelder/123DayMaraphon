using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVDroneTriggeredDeath : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerFallTrigger>(out PlayerFallTrigger fallTrigger))
        {
            Destroy(_parent);
        }
    }
}
