using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PUSBlowJobSound : MonoBehaviour
{
    [SerializeField] private PUSShootState _pusShootState;

    [SerializeField] private AudioSource _audio;

    private void OnEnable()
    {
        _pusShootState.StartedShooting += OnStartedShooting;
        _pusShootState.StoppedShooting += OnStoppedShooting;
    }

    private void OnDisable()
    {
        _pusShootState.StartedShooting -= OnStartedShooting;
        _pusShootState.StoppedShooting -= OnStoppedShooting;
    }

    private void OnStartedShooting()
    {
        _audio.Play();
    }
    
    private void OnStoppedShooting()
    {
        _audio.Stop();
    }
}
