using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public event Action<float> OnFOVChanged;
    [SerializeField] private CinemachineVirtualCamera _cinemachine;
    [SerializeField] private Camera _overlayCamera;

    public void ChangeFOV(float value)
    {
        _cinemachine.m_Lens.FieldOfView = value;
        _overlayCamera.fieldOfView = value;
        OnFOVChanged?.Invoke(value);
    }
}
