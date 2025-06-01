using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemDifficultyChange : MonoBehaviour
{
    [SerializeField] private DifficultyHandler _difficulty;

    private void OnEnable()
    {
        _difficulty.DifficultyChanged += OnDifficultyChanged;
    }

    private void OnDifficultyChanged(Difficulty obj)
    {
        
    }

    private void OnDisable()
    {
        
    }
}