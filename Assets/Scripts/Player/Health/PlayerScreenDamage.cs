using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerScreenDamage : MonoBehaviour
{
    [Inject] private PlayerHealth _health;
    [SerializeField] private Image _damageScreen;
    [SerializeField] private float _speed = 5f;

    private float _currentAlpha;
    private float alpha;

    private void OnEnable()
    {
        _health.HealthValueChanged += OnHealthValueChanged;
    }

    private void Update()
    {
        alpha += Time.deltaTime / _speed;
        if (alpha > 2.0f * Mathf.PI)
            alpha -= 2.0f * Mathf.PI;
        float calculateAlpha = Mathf.Sin(alpha);

        calculateAlpha = Mathf.Abs(calculateAlpha);

        Color imageColor = _damageScreen.color;
        imageColor.a = _currentAlpha * calculateAlpha;
        _damageScreen.color = imageColor;
    }

    private void OnHealthValueChanged(float health)
    {
        if (health <= 20)
        {
            _currentAlpha = 0.7f;
        }
        else if (health <= 50)
        {
            _currentAlpha = 0.35f;
        }
        else if (health <= 70)
        {
            _currentAlpha = 0.2f;
        }
        else
        {
            _currentAlpha = 0;
        }
    }

    private void OnDisable()
    {
        _health.HealthValueChanged -= OnHealthValueChanged;
    }
}