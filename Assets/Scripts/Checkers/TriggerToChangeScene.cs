using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToChangeScene : PlayerTrigger
{
    public override void Triggered()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("SceneSwitcher");
    }
}