using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _minAmmo = 0;
    [SerializeField] private bool _fullOnStart;

    protected int CurrentAmmo { get; private set; } = 0;

    public event Action<int> ValueChanged;
    public event Action<int> Spended;

    private void Start()
    {
        if (_fullOnStart)
        {
            CurrentAmmo = _maxAmmo;
            ValueChanged?.Invoke(CurrentAmmo);
        }
    }

    public bool IsOut() => CurrentAmmo == _minAmmo;

    public virtual void SpendAmmo(int value)
    {
        if (CurrentAmmo - value >= _minAmmo)
        {
            CurrentAmmo -= value;
            ValueChanged?.Invoke(CurrentAmmo);
            Spended?.Invoke(CurrentAmmo);
            return;
        }

        CurrentAmmo = _minAmmo;
        Spended?.Invoke(CurrentAmmo);
        ValueChanged?.Invoke(CurrentAmmo);
    }

    public virtual void AddAmmo(int value)
    {
        if (CurrentAmmo + value <= _maxAmmo)
        {
            CurrentAmmo += value;
            ValueChanged?.Invoke(CurrentAmmo);
            return;
        }

        CurrentAmmo = _maxAmmo;
        ValueChanged?.Invoke(CurrentAmmo);
    }
}