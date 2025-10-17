using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveNumberText;

    [SerializeField] private Wave _wave;

    private void OnEnable()
    {
        _wave.TimerCounted += OnTimerCounted;

        _wave.Started += OnStarted;
    }

    private void OnStarted(int value)
    {
        _waveNumberText.text = value.ToString();
    }

    private void OnTimerCounted(long value)
    {
        _waveText.text = value.ToString();
    }

    private void OnDisable()
    {
        _wave.TimerCounted -= OnTimerCounted;

        _wave.Started -= OnStarted;
    }
}