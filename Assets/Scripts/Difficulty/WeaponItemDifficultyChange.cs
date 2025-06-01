using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemDifficultyChange : MonoBehaviour
{
    [SerializeField] private DifficultyHandler _difficulty;
    [SerializeField] private WeaponItem[] _items;

    private void OnEnable()
    {
        _difficulty.DifficultyChanged += OnDifficultyChanged;
        OnDifficultyChanged(_difficulty.CurrentDifficulty);
    }

    private void OnDifficultyChanged(Difficulty difficulty)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].MultipliDamage(difficulty.DamageMultiplier);
        }
    }

    private void OnDisable()
    {
        _difficulty.DifficultyChanged -= OnDifficultyChanged;
    }
}