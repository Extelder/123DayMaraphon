using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using SlimUI.ModernMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


[Serializable]
public class OutlineRender
{
    public int QualityLevel;
    public ScriptableRendererFeature ScriptableRendererFeature;
}

public class Settings : MonoBehaviour
{
    [Inject] private Pools _pools;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;
    [SerializeField] private ThemedUIData _themeController;
    [SerializeField] private PlayerFOV _playerFOV;

    [SerializeField] private Slider _sensetivitySlider;
    [field:SerializeField] public Slider MasterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private Slider _fovSlider;

    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    [SerializeField] private PlayerDeath _death;

    [SerializeField] private string _onText;
    [SerializeField] private string _offText;
    [SerializeField] private TextMeshProUGUI _outlineSwitchText;

    [SerializeField] private OutlineRender[] _outlineRenders;
    [SerializeField] private TextMeshProUGUI _fullScreenOnOffTText;
    [SerializeField] private TextMeshProUGUI _bloodSplatOnOffTText;

    [SerializeField] private TextMeshProUGUI _sensetivityValueText;
    [SerializeField] private TextMeshProUGUI _fovValueText;
    [SerializeField] private TextMeshProUGUI _musicVolumeValueText;
    [SerializeField] private TextMeshProUGUI _effectsVolumeValueText;
    [SerializeField] private TextMeshProUGUI _masterVolumeValueText;

    [SerializeField] private bool _hub;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private float _currentRefrashRate;
    private int _currentResolutionIndex = 0;
    private CinemachinePOV _cameraModule;

    public event Action Opened;
    public event Action Closed;

    public bool Open { get; private set; } = false;

    public static Settings Instance { get; private set; }

    public event Action<float> MasterValueChanged;
    public event Action<float> EffectsValueChanged;

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

        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        _effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1);
        _fovSlider.value = PlayerPrefs.GetFloat("FOV", 100);
        if (!_hub)
            _playerFOV.ChangeFOV(PlayerPrefs.GetFloat("FOV", 100));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 3));
        bool fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen", 1));
        Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), fullScreen);
        SetResolutionReady();
        Screen.fullScreen = fullScreen;
        if (fullScreen)
        {
            _fullScreenOnOffTText.text = _offText;
        }
        else
        {
            _fullScreenOnOffTText.text = _onText;
        }


        if (PlayerPrefs.GetInt("BloodSplat", 1) == 0)
        {
            if (!_hub)
                _pools.BloodSplatPool.transform.localScale = Vector3.zero;
            _bloodSplatOnOffTText.text = _offText;
        }
        else
        {
            if (!_hub)
                _pools.BloodSplatPool.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _bloodSplatOnOffTText.text = _onText;
        }
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
        _sensetivityValueText.text = value.ToString("0.00");
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

        _masterVolumeValueText.text = value.ToString("0.00");
        MasterValueChanged?.Invoke(value);
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

        _musicVolumeValueText.text = value.ToString("0.00");
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

        _effectsVolumeValueText.text = value.ToString("0.00");
        EffectsValueChanged?.Invoke(value);
    }

    public void QualityChange(int value)
    {
        bool active = false;
        for (int i = 0; i < _outlineRenders.Length; i++)
        {
            if (QualitySettings.GetQualityLevel() == _outlineRenders[i].QualityLevel)
            {
                active = _outlineRenders[i].ScriptableRendererFeature.isActive;
            }
        }

        PlayerPrefs.SetInt("Quality", value);
        QualitySettings.SetQualityLevel(value);
        for (int i = 0; i < _outlineRenders.Length; i++)
        {
            if (QualitySettings.GetQualityLevel() == _outlineRenders[i].QualityLevel)
            {
                _outlineRenders[i].ScriptableRendererFeature.SetActive(active);

                if (_outlineRenders[i].ScriptableRendererFeature.isActive)
                {
                    _outlineSwitchText.text = _offText;
                }
                else
                {
                    _outlineSwitchText.text = _onText;
                }

                return;
            }
        }
    }

    public void SwitchOutlineSettings(TextMeshProUGUI text)
    {
        for (int i = 0; i < _outlineRenders.Length; i++)
        {
            if (QualitySettings.GetQualityLevel() == _outlineRenders[i].QualityLevel)
            {
                Debug.Log(_outlineRenders[i].ScriptableRendererFeature.isActive);
                if (_outlineRenders[i].ScriptableRendererFeature.isActive)
                {
                    text.text = _onText;
                    _outlineRenders[i].ScriptableRendererFeature.SetActive(false);
                }
                else
                {
                    text.text = _offText;
                    _outlineRenders[i].ScriptableRendererFeature.SetActive(true);
                }

                Debug.Log(_outlineRenders[i].ScriptableRendererFeature.isActive);
                return;
            }
        }
    }

    public void SetResolutionReady()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        _resolutionDropdown.ClearOptions();
        _currentRefrashRate = Screen.currentResolution.refreshRate;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].refreshRate == _currentRefrashRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " +
                                      _filteredResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void ChangeResolutions(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        PlayerPrefs.SetInt("Height", resolution.height);
        PlayerPrefs.SetInt("Width", resolution.width);
    }

    public void SwitchFullScreenSettings()
    {
        if (!Screen.fullScreen)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("FullScreen", 0);
        }
    }

    public void ChangeFOV(float value)
    {
        PlayerPrefs.SetFloat("FOV", value);
        _fovValueText.text = Mathf.Round(value).ToString();
        if (!_hub)
        {
            _playerFOV.ChangeFOV(value);
        }
    }

    public void EnableDisableBlood()
    {
        if (PlayerPrefs.GetInt("BloodSplat") == 1)
        {
            if (!_hub)
                _pools.BloodSplatPool.transform.localScale = Vector3.zero;
            PlayerPrefs.SetInt("BloodSplat", 0);
        }
        else
        {
            if (!_hub)
                _pools.BloodSplatPool.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            PlayerPrefs.SetInt("BloodSplat", 1);
        }
    }

    public void OnOffText(TextMeshProUGUI text)
    {
        if (text.text == _onText)
            text.text = _offText;
        else
            text.text = _onText;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}