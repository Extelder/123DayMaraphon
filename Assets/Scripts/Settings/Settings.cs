using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using SlimUI.ModernMenu;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;
    [SerializeField] private ThemedUIData _themeController;

    [SerializeField] private Slider _sensetivitySlider;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private PlayerDeath _death;

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

            _cameraModule = _cinemachineVirtual?.GetCinemachineComponent<CinemachinePOV>();
            return;
        }

        Debug.LogError(gameObject);
        Debug.LogError("There`s one more settings");
        Debug.Break();
    }

    private void Start()
    {
        _themeController.currentColor = _themeController.custom1.graphic1;

        _sensetivitySlider.value = PlayerPrefs.GetFloat("Sensetivity", 1f) / 2;
        if (_cameraModule != null) _cameraModule.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Sensetivity", 1f);
        if (_cameraModule != null) _cameraModule.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Sensetivity", 1f);

        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectVolume");
    }

    public void OpenCloseSettings()
    {
        if (_death != null)
            if (_death.Dead)
                return;
        _settingsCanvas.SetActive(!_settingsCanvas.activeSelf);
        Open = _settingsCanvas.activeSelf;
        if (_settingsCanvas.activeSelf)
        {
            PlayerTime.Instance.PauseTimeStop();
            Opened?.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            Closed?.Invoke();
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseSettings();
        }
    }

    public void OnSensetivitySliderValueChanged(float value)
    {
        if (_cameraModule != null) _cameraModule.m_VerticalAxis.m_MaxSpeed = value * 2;
        if (_cameraModule != null) _cameraModule.m_HorizontalAxis.m_MaxSpeed = value * 2;
        PlayerPrefs.SetFloat("Sensetivity", value * 2);
    }

    public void OnMasterVolumeSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        if (value == 0)
        {
            _mixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }
    }

    public void OnMusicVolumeSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (value == 0)
        {
            _mixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }
    }

    public void OnEffectsVolumeSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("EffectVolume", value);

        if (value == 0)
        {
            _mixer.SetFloat("EffectVolume", -80);
        }
        else
        {
            _mixer.SetFloat("EffectVolume", Mathf.Log10(value) * 20);
        }
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