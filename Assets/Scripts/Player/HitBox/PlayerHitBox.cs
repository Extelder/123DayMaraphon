using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerHitBox : MonoBehaviour
{
    [Inject] private PlayerHealth _health;

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }
}