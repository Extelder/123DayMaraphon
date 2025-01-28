using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;

    [SerializeField] private Scrollbar _sensetivitySlider;
    [SerializeField] private Scrollbar _volumeSlider;

    private CinemachinePOV _cameraModule;

    public event Action Opened;
    public event Action Closed;

    public bool Open { get; private set; } = false;

    public static Settings Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

            _cameraModule = _cinemachineVirtual.GetCinemachineComponent<CinemachinePOV>();
            return;
        }

        Debug.LogError(gameObject);
        Debug.LogError("There`s one more settings");
        Debug.Break();
    }

    private void Start()
    {
        _sensetivitySlider.value = PlayerPrefs.GetFloat("Sensetivity", 1f) / 2;
        if (_cameraModule != null) _cameraModule.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Sensetivity", 1f);
        if (_cameraModule != null) _cameraModule.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Sensetivity", 1f);

        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.4f);
        _volumeSlider.value = AudioListener.volume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _settingsCanvas.SetActive(!_settingsCanvas.activeSelf);
            Open = _settingsCanvas.activeSelf;
            if (_settingsCanvas.activeSelf)
            {
                Opened?.Invoke();
            }
            else
            {
                Closed?.Invoke();
            }
        }
    }

    public void OnSensetivitySliderValueChanged(float value)
    {
        if (_cameraModule != null) _cameraModule.m_VerticalAxis.m_MaxSpeed = value * 2;
        if (_cameraModule != null) _cameraModule.m_HorizontalAxis.m_MaxSpeed = value * 2;
        PlayerPrefs.SetFloat("Sensetivity", value * 2);
    }

    public void OnSoundSliderValueChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}