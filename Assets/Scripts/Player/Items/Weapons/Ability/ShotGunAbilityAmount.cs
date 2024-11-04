using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ShotGunAbilityAmount : WeaponAbilityAmount
{
    [SerializeField] private GameObject _ghost;
    [SerializeField] private ShotGunAbility _shotGunAbility;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Filled.Subscribe(filled => { Debug.Log(filled); _ghost.SetActive(filled); }).AddTo(_disposable);
        _shotGunAbility.AbilityPerformed += OnAbilityPerformed;
    }

    private void OnAbilityPerformed()
    {
        SpendAll();
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _shotGunAbility.AbilityPerformed -= OnAbilityPerformed;
    }
}