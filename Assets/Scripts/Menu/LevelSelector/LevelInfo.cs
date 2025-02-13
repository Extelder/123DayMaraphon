using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [field: SerializeField] public int LevelBuildIndex { get; private set; }
    [field: SerializeField] public GameObject Picture { get; private set; }
    [field: SerializeField] public string LevelName { get; private set; }
    [field: SerializeField] public Collider Collider { get; private set; }

    [SerializeField] private GameObject _locked;

    private void Start()
    {
        PlayerPrefs.SetString("Magic Missile", "Unlocked");
        if (PlayerPrefs.GetString(LevelName) != "Unlocked")
        {
            _locked.SetActive(false);
        }
    }
}