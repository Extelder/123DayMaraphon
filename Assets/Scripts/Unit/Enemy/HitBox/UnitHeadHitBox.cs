using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitHeadHitBox : UnitHitBox
{
    [SerializeField] private float _damageMultiply = 2;

    public override void TakeDamage(float damage, HypeType hypeType, float multiplier)
    {
        base.TakeDamage(damage * _damageMultiply, HypeType.HeadKill, multiplier * _damageMultiply);
    }
}