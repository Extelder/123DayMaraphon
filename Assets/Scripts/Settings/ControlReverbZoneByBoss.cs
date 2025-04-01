using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControlReverbZoneByBoss : MonoBehaviour
{
    private Settings _settings;
    private float _masterValue;
    private float _effectsValue;

    private void Start()
    {
        Invoke(nameof(OnBossTrigerEnter), 3f);
    }

    public void OnBossTrigerEnter()
    {
        _settings = Settings.Instance;
        _settings.MasterValueChanged += OnMasterValueChanged;
        _settings.EffectsValueChanged += OnEffectsValueChanged;
        _masterValue = PlayerPrefs.GetFloat("MasterVolume", 1);
        _effectsValue = PlayerPrefs.GetFloat("EffectVolume", 1);
        EnableDisableReverbZone();
    }

    private void OnMasterValueChanged(float masterValue)
    {
        _masterValue = masterValue;
        EnableDisableReverbZone();
    }

    private void EnableDisableReverbZone()
    {
        if (_masterValue == 0 || _effectsValue == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    
    private void OnEffectsValueChanged(float effectsValue)
    {
        _effectsValue = effectsValue;
        EnableDisableReverbZone();
    }

    private void OnDisable()
    {
        _settings.MasterValueChanged -= OnMasterValueChanged;
        _settings.EffectsValueChanged -= OnEffectsValueChanged;
    }
}
