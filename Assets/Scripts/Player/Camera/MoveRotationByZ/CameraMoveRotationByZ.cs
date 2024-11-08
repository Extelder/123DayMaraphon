using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraMoveRotationByZ : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private float _timeToDefaultValue;
    [SerializeField] private float _angle;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    [Inject] private PlayerInputs _controls;

    private void Update()
    {
        if (_controls.PlayerMovementInputs.MovementHorizontal > 0)
        {
            _cinemachineVirtualCamera.m_Lens.Dutch =
                Mathf.Lerp(_cinemachineVirtualCamera.m_Lens.Dutch, -_angle, _time * Time.deltaTime);
            return;
        }

        if (_controls.PlayerMovementInputs.MovementHorizontal < 0)
        {
            _cinemachineVirtualCamera.m_Lens.Dutch =
                Mathf.Lerp(_cinemachineVirtualCamera.m_Lens.Dutch, _angle, _time * Time.deltaTime);
            return;
        }

        if (Mathf.Abs(_cinemachineVirtualCamera.m_Lens.Dutch) < 0.1f)
        {
            _cinemachineVirtualCamera.m_Lens.Dutch = 0;
            return;
        }

        _cinemachineVirtualCamera.m_Lens.Dutch =
            Mathf.Lerp(_cinemachineVirtualCamera.m_Lens.Dutch, 0, _timeToDefaultValue * Time.deltaTime);
    }
}