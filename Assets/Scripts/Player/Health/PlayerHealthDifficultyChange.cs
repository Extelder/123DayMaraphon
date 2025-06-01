using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDifficultyChange : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;

    private void OnEnable()
    {
        DifficultyHandler.Instance.DifficultyChanged += OnDifficultyChanged;
        OnDifficultyChanged(DifficultyHandler.Instance.CurrentDifficulty);
    }

    private void OnDifficultyChanged(Difficulty difficulty)
    {
    }

    private void OnDisable()
    {
        DifficultyHandler.Instance.DifficultyChanged -= OnDifficultyChanged;
    }
}