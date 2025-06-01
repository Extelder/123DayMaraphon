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
    public float DefaultDamagePerHit;

    public void ResetDamage()
    {
        DamagePerHit = DefaultDamagePerHit;
    }

    public void MultipliDamage(float multiplier)
    {
        ResetDamage();
        DamagePerHit *= multiplier;
    }
}