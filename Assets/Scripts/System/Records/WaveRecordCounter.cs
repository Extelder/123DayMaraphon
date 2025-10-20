using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRecordCounter : MonoBehaviour
{
    private Wave _wave;
    private int _currentWave;

    private void Start()
    {
        _wave = Wave.Instance;
        _wave.Started += OnWaveStarted;
    }
    
    private void OnWaveStarted(int currentWave)
    {
        _currentWave = currentWave;
        if (PlayerPrefs.GetInt("MaxWave", 0) < _currentWave)
            PlayerPrefs.SetInt("MaxWave", _currentWave);
    }

    private void OnDisable()
    {
        _wave.Started -= OnWaveStarted;
    }
}
