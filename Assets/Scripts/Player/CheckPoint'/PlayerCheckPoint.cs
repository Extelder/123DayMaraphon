using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayerCheckPoint : PlayerTrigger
{
    [Inject] private PlayerCharacter _character;

    public UnityEvent Trigger;

    private Collider _collider;

    private void Awake()
    {
        Debug.LogError(PlayerPrefs.GetString(SceneManager.GetActiveScene().name) + " CheckPoint");
        _collider = GetComponent<Collider>();
        if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name) == gameObject.name)
        {
            Debug.LogError(transform.position);
            _character.Transform.position = transform.position;
        }
    }

    public override void OnTriggered()
    {
        _collider.enabled = false;

        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, gameObject.name);
        Trigger?.Invoke();
    }
}