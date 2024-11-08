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
    [SerializeField] private GameObject _railgun;

    [Inject] private PlayerInputs _inputs;

    private GameObject _currentWeapon;

    private void OnEnable()
    {
        _inputs.PlayerSwitchWeaponInputs.ShotGunKeyPressedDown += ShotGun;
        _inputs.PlayerSwitchWeaponInputs.RifleKeyPressedDown += Rifle;
        _inputs.PlayerSwitchWeaponInputs.RPGKeyPressedDown += RPG;
        _inputs.PlayerSwitchWeaponInputs.RailgunKeyPressedDown += Railgun;
    }

    private void OnDisable()
    {
        _inputs.PlayerSwitchWeaponInputs.ShotGunKeyPressedDown -= ShotGun;
        _inputs.PlayerSwitchWeaponInputs.RifleKeyPressedDown -= Rifle;
        _inputs.PlayerSwitchWeaponInputs.RPGKeyPressedDown -= RPG;
        _inputs.PlayerSwitchWeaponInputs.RailgunKeyPressedDown -= Railgun;
    }

    public void ShotGun() => ChangeWeapon(_shotGun);
    public void Rifle() => ChangeWeapon(_rifle);
    public void RPG() => ChangeWeapon(_rpg);
    public void Railgun() => ChangeWeapon(_railgun);

    public void ChangeWeapon(GameObject weapon)
    {
        if (_currentWeapon == weapon)
            return;
        _currentWeapon?.SetActive(false);
        _currentWeapon = weapon;
        _currentWeapon.SetActive(true);
    }
}