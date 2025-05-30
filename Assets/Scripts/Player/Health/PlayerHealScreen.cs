using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealScreen : MonoBehaviour
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private string _triggerName;
    [SerializeField] private Animator _bloodScreanAnimator;

    private void OnEnable()
    {
        _playerHealth.Healed += OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float value)
    {
        _bloodScreanAnimator.SetTrigger(_triggerName);
    }

    private void OnDisable()
    {
        _playerHealth.Healed -= OnHealthValueChanged;
    }
}