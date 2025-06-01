using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/ New Weapon")]
public class WeaponItem : ScriptableObject
{
    public string Name;
    public int Id;
    public float DamagePerHit;

    private float _defaultDamage;


    private void OnDisable()
    {
        DamagePerHit = _defaultDamage;
    }

    public void ResetDamage()
    {
        DamagePerHit = _defaultDamage;
    }

    public void MultipliDamage(float multiplier)
    {
        _defaultDamage = DamagePerHit;
        DamagePerHit *= multiplier;
    }
}