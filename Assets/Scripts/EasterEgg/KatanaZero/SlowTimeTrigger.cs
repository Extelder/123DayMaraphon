using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeTrigger : PlayerTrigger
{
    [SerializeField] private float _slowedTime = 0.7f;
    [SerializeField] private AudioSource _musicVolume;

    public override void OnTriggered()
    {
        Time.timeScale = _slowedTime;
        _musicVolume.pitch = 0.5f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Time.timeScale = 1;
            _musicVolume.pitch = 1f;
        }
    }
}