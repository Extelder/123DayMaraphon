using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWeaponUnlocker : MonoBehaviour
{
    [SerializeField] private string[] _weaponsOnLevel;

    private void Start()
    {
        for (int i = 0; i < _weaponsOnLevel.Length; i++)
        {
            PlayerPrefs.SetString(_weaponsOnLevel[i], "Unlocked");
        }
    }
}