using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpenSceneSwitcher : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private string _sceneToSwitchName;

    private void OnEnable()
    {
        _door.Opened += OnOpened;
    }

    private void OnDisable()
    {
        _door.Opened -= OnOpened;
    }

    private void OnOpened()
    {
        SceneManager.LoadScene(_sceneToSwitchName);
    }
}