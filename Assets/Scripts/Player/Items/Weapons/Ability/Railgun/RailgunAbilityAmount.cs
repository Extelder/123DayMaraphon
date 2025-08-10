using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RailgunAbilityAmount : WeaponAbilityAmount
{
    [SerializeField] private MeshRenderer _railgun;
    [SerializeField] private Material _railgunReadyMaterial;
    [SerializeField] private RailgunAbility _railgunAbility;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Filled.Subscribe(filled => { _railgun.material = _railgunReadyMaterial; }).AddTo(_disposable);
        _railgunAbility.AbilityPerformed += OnAbilityPerformed;
    }

    private void OnAbilityPerformed()
    {
        SpendAll();
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _railgunAbility.AbilityPerformed -= OnAbilityPerformed;
    }
}