using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUSHealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Text _text;
    [SerializeField] private Image _healthBar;

    private bool _aba;
    
    private void OnEnable()
    {
        _health.HealthValueChanged += OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float value)
    {
        _healthBar.fillAmount = value / 10000;
        _text.text = value.ToString();
    }

    private void OnDisable()
    {
        _health.HealthValueChanged -= OnHealthValueChanged;
    }
}