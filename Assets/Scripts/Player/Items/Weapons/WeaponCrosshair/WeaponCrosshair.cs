using System;
using UnityEngine;

public class WeaponCrosshair : MonoBehaviour
{
    [SerializeField] private PlayerHip _hip;
    [SerializeField] private GameObject _crosshair;

    private void OnEnable()
    {
        _hip.ChangeHip(_crosshair);
    }
}