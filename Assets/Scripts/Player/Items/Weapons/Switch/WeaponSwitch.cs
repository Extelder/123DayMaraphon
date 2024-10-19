using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private WeaponStateMachine[] _weaponsStateMachines;
    [SerializeField] private float _switchRate;
    [SerializeField] private GameObject? _currentWeapon;

    private int _lastWeaponIndex = 0;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        StartCoroutine(SwitchingWeapons());
        ChangeWeapon(_weapons[0]);
    }

    private IEnumerator SwitchingWeapons()
    {
        while (true)
        {
            yield return new WaitForSeconds(_switchRate);

            _lastWeaponIndex = _currentWeaponIndex;
            if (GetRandomWeaponId(out GameObject weapon))
            {
                yield return new WaitUntil(() => !_weaponsStateMachines[_currentWeaponIndex].CurrentState.CanChanged);
                Debug.Log(_weaponsStateMachines[_currentWeaponIndex]);
                ChangeWeapon(weapon);
            }
        }
    }

    private bool GetRandomWeaponId(out GameObject weapon)
    {
        int randWeaponId = Random.Range(0, _weapons.Length);
        if (_weapons[randWeaponId] == _currentWeapon)
        {
            return GetRandomWeaponId(out weapon);
        }

        weapon = _weapons[randWeaponId];
        _currentWeaponIndex = randWeaponId;
        return true;
    }


    public void ChangeWeapon(GameObject weapon)
    {
        if (_currentWeapon == weapon)
            return;
        _currentWeapon?.SetActive(false);
        _currentWeapon = weapon;
        _currentWeapon.SetActive(true);
    }
}