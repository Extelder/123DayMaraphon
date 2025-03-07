using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerBloodScrean : MonoBehaviour
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private string _triggerName;
    [SerializeField] private Animator _bloodScreanAnimator;

    private void OnEnable()
    {
        _playerHealth.HealthValueChanged += OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float value)
    {
        _bloodScreanAnimator.SetTrigger(_triggerName);
    }

    private void OnDisable()
    {
        _playerHealth.HealthValueChanged -= OnHealthValueChanged;
    }
}
