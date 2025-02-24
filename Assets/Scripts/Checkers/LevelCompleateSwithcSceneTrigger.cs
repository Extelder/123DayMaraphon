using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleateSwithcSceneTrigger : PlayerTrigger
{
    [SerializeField] private Level _level;

    public override void Triggered()
    {
        _level.CompleateLevel();
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("SceneSwitcher");
    }
}