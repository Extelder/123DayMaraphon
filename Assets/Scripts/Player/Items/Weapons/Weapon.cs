using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] public bool Unlocked { get; private set; }
    [field: SerializeField] public GameObject WeaponObject { get; private set; }

    private void Awake()
    {
        if (PlayerPrefs.GetString(gameObject.name, "Locked") == "Unlocked")
        {
            Unlocked = true;
        }
    }

    public void Unlock()
    {
        Unlocked = true;
        PlayerPrefs.SetString(gameObject.name, "Unlocked");
    }
}