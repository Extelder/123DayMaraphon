using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Door _door;
    [SerializeField] private bool _destroyAfterInteract;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "Pickuped") == 1)
        {
            _door.UnLock();
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        _door.UnLock();
        PlayerPrefs.SetInt(gameObject.name + "Pickuped", 1);
        if (_destroyAfterInteract)
            Destroy(gameObject);
    }
}