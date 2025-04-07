using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHype : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;

    private void Start()
    {
        _health.PlayerDamaged += OnPlayerDamaged;
    }

    private void OnDisable()
    {
        _health.PlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged(float damage)
    {
        float convertedDamage = damage / 100 * 2;
        Debug.Log(convertedDamage);
        PlayerHypeSystem.Instance.Remove(convertedDamage);
    }
}