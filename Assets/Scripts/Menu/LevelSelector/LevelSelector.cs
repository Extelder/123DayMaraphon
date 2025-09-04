using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private AudioSource _insertSound;
    [SerializeField] private int _allLevels;

    private LevelInfo _currentLevelInfo;
    private int _levelNumber;

    public void SendCurrentLevelInfo(LevelInfo levelInfo)
    {
        _playButton.SetActive(false);
        if (_currentLevelInfo != null)
        {
            _currentLevelInfo.Collider.enabled = true;

            _currentLevelInfo.Picture.SetActive(false);
            _text.enabled = false;
        }


        _currentLevelInfo = levelInfo;
        _currentLevelInfo.Collider.enabled = false;

        _text.text = _currentLevelInfo.LevelName;
    }

    public void ShowCurrentLevel()
    {
        _insertSound.Play();
        _currentLevelInfo.Collider.enabled = false;
        _currentLevelInfo.Picture.SetActive(true);
        _playButton.SetActive(true);
        _text.enabled = true;
    }

    public void RetryCurrentLevel()
    {
        PlayerPrefs.SetString(_currentLevelInfo.LevelBuildIndex + "Level", "");
        PlayerPrefs.SetInt("CurrentScene", _currentLevelInfo.LevelBuildIndex);
        SceneManager.LoadScene("SceneSwitcher");
    }

    public void PlayCurrentLevel()
    {
        PlayerPrefs.SetInt("CurrentScene", _currentLevelInfo.LevelBuildIndex);
        SceneManager.LoadScene("SceneSwitcher");
    }
}