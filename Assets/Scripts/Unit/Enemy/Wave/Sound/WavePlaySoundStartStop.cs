using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WavePlaySoundStartStop : MonoBehaviour
{
    [Inject] private Pools _pools;
    [SerializeField] private EnemyWaveSystem _waveSystem;
    private void OnEnable()
    {
        _waveSystem.WavingStarted += OnWaveStarted;
        _waveSystem.WavingStarted += OnWaveStoped;
    }

    private void OnWaveStarted()
    {
        _pools.WaveStartedSoundPool.GetFreeElement(transform.position);
    }
    
    private void OnWaveStoped()
    {
        _pools.WaveStopedSoundPool.GetFreeElement(transform.position);
    }

    private void OnDisable()
    {
        _waveSystem.WavingStarted -= OnWaveStarted;
        _waveSystem.WavingStarted -= OnWaveStoped;
    }
}
