using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoAutoRecover : MonoBehaviour
{
    [SerializeField] private WeaponAmmo _ammo;
    [SerializeField] private float _recoverRate;
    [SerializeField] private int _recoverAmount;

    private void OnEnable()
    {
        _ammo.Spended += OnSpended;
    }

    private void Start()
    {
        StartCoroutine(Recovering());
    }

    private void OnDisable()
    {
        _ammo.Spended -= OnSpended;
    }

    private void OnSpended(int value)
    {
        StopAllCoroutines();
        StartCoroutine(Recovering());
    }

    private IEnumerator Recovering()
    {
        while (true)
        {
            yield return new WaitForSeconds(_recoverRate);
            _ammo.AddAmmo(_recoverAmount);
        }
    }
}