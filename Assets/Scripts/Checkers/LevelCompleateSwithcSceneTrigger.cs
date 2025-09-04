using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleateSwithcSceneTrigger : PlayerTrigger
{
    [SerializeField] private Level _level;

    public override void OnTriggered()
    {
        if (PlayerCharacter.Instance.PlayerDeath.Deaths == 0)
        {
            Debug.Log("PerfectlyPassed" + SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt("PerfectlyPassed" + SceneManager.GetActiveScene().buildIndex, 1);
        }
        _level.CompleateLevel();
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("SceneSwitcher");
    }
}