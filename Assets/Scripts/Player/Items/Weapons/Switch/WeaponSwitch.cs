using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _shotGun;
    [SerializeField] private GameObject _rifle;
    [SerializeField] private GameObject _rpg;
    
    [Inject] private PlayerInputs _inputs;

    private GameObject _currentWeapon;
    
    private void OnEnable()
    {
        _inputs.ShotGunKeyPressedDown += ShotGun;
        _inputs.RifleKeyPressedDown += Rifle;
        _inputs.RPGKeyPressedDown += RPG;
    }

    private void OnDisable()
    {
        _inputs.ShotGunKeyPressedDown -= ShotGun;
        _inputs.RifleKeyPressedDown -= Rifle;
        _inputs.RPGKeyPressedDown -= RPG;
    }

    public void ShotGun() => ChangeWeapon(_shotGun);
    public void Rifle() => ChangeWeapon(_rifle);
    public void RPG() => ChangeWeapon(_rpg);

    public void ChangeWeapon(GameObject weapon)
    {
        if (_currentWeapon == weapon)
            return;
        _currentWeapon?.SetActive(false);
        _currentWeapon = weapon;
        _currentWeapon.SetActive(true);
    }
}