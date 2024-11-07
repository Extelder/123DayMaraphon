using Zenject;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private Text _healthText;

    private void Awake()
    {
        OverrideHealth(_playerHealth);
    }

    public override void OnHealthValueChanged(float value)
    {
        base.OnHealthValueChanged(value);
        _healthText.text = value.ToString();
    }
}