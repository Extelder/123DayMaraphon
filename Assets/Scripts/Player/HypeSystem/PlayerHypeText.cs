using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

[Serializable]
public struct HypeText
{
    [field: SerializeField] public int HypeValue { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
}

public class PlayerHypeText : MonoBehaviour
{
    [SerializeField] private PlayerHypeSystem _hypeSystem;

    [SerializeField] private HypeText[] _hypeTexts;

    private HypeText _currentHypeText;

    private void OnEnable()
    {
        _currentHypeText = _hypeTexts[0];
        _hypeSystem.HypeChanged += OnHypeChanged;
    }

    private void OnHypeChanged(float value)
    {
        _currentHypeText.Text.enabled = false;

        for (int i = 0; i < _hypeTexts.Length; i++)
        {
            if (value >= _hypeTexts[i].HypeValue)
            {
                _currentHypeText = _hypeTexts[i];
            }
        }

        _currentHypeText.Text.enabled = true;
    }

    private void OnDisable()
    {
        _hypeSystem.HypeChanged += OnHypeChanged;
    }
}