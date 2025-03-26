using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PUSMoveSound : MonoBehaviour
{
    [SerializeField] private PUSMoveState _pusMoveState;
    [SerializeField] private PUSMoveUpState _pusMoveUpState;
    [SerializeField] private AudioSource _movingAudio;

    private void OnEnable()
    {
        _pusMoveState.PUSStartedMoving += OnPusStartedMoving;
        _pusMoveState.PUSStopedMoving += OnPusStoppedMoving;

        _pusMoveUpState.PUSStartedMovingUp += OnPusStartedMoving;
        _pusMoveUpState.PUSStopedMovingUp += OnPusStoppedMoving;
    }

    private void OnDisable()
    {
        _pusMoveState.PUSStartedMoving -= OnPusStartedMoving;
        _pusMoveState.PUSStopedMoving -= OnPusStoppedMoving;
        
        
        _pusMoveUpState.PUSStartedMovingUp -= OnPusStartedMoving;
        _pusMoveUpState.PUSStopedMovingUp -= OnPusStoppedMoving;
    }

    private void OnPusStartedMoving()
    {
        _movingAudio.Play();
    }

    private void OnPusStoppedMoving()
    {
        _movingAudio.Stop();
    }
}
