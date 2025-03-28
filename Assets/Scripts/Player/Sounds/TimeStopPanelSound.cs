using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

public class TimeStopPanelSound : MonoBehaviour
{
    [SerializeField] private KunitanShoot _kunitanShoot;

    [SerializeField] private AudioSource _timeStopSound;

    private void OnEnable()
    {
        _kunitanShoot.TimeStopped += OnTimeStopped;
    }

    private void OnDisable()
    {
        _kunitanShoot.TimeStopped -= OnTimeStopped;
    }

    private void OnTimeStopped()
    {
        _timeStopSound.Play();
        Debug.Log("Time Stopped");
    }
}
