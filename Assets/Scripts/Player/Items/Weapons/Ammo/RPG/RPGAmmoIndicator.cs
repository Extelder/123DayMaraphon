using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGAmmoIndicator : MonoBehaviour
{
    [SerializeField] private GameObject[] _indicators;
    [SerializeField] private Ammo _ammo;

    private void OnEnable()
    {
        _ammo.ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(int value)
    {
        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].SetActive(false);
        }

        for (int i = 0; i < value; i++)
        {
            _indicators[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        _ammo.ValueChanged -= OnValueChanged;
    }
}