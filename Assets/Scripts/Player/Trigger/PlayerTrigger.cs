using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTrigger : MonoBehaviour
{
    [field: SerializeField] public bool DestroyGameObjectAfterTriggered = true;
    [field: SerializeField] public bool DestroyComponentAfterTriggered;


    public event Action Triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Triggered?.Invoke();
            OnTriggered();
            if (DestroyGameObjectAfterTriggered)
                Destroy(gameObject);
            if (DestroyComponentAfterTriggered)
                Destroy(this);
        }
    }

    public abstract void OnTriggered();
}