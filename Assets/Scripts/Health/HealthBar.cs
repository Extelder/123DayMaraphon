using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healtBar;
    [SerializeField] private Health _health;

    protected void OverrideHealth(Health health)
    {
        _health = health;
    }

    private void OnEnable()
    {
        _health.HealthValueChanged += OnHealthValueChanged;
    }

    private void OnDisable()
    {
        _health.HealthValueChanged -= OnHealthValueChanged;
    }

    public virtual void OnHealthValueChanged(float value)
    {
        float percent = _health.MaxValue / 100;
        _healtBar.fillAmount = value / percent / 100;
    }
}