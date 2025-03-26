using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitHeadHitBox : UnitHitBox
{
    [SerializeField] private float _damageMultiply = 2;

    public override void TakeDamage(float damage, float multiplier)
    {
        base.TakeDamage(damage * _damageMultiply, multiplier * _damageMultiply);
    }
}