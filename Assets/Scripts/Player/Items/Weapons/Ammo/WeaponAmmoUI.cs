using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponAmmoUI : MonoBehaviour
{
    [SerializeField] private WeaponAmmo _ammo;
    [SerializeField] private TextMeshPro _text;

    private void OnEnable()
    {
        _ammo.ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(int value)
    {
        _text.text = value.ToString();
    }

    private void OnDisable()
    {
        _ammo.ValueChanged -= OnValueChanged;
    }
}