using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RPGAbilityAmount : WeaponAbilityAmount
{
    [SerializeField] private RPGAbility _rpgAbility;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Filled.Subscribe(filled => {}).AddTo(_disposable);
        _rpgAbility.AbilityPerformed += OnAbilityPerformed;
    }

    private void OnAbilityPerformed()
    {
        SpendAll();
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _rpgAbility.AbilityPerformed -= OnAbilityPerformed;
    }
}
