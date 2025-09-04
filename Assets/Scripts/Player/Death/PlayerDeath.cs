using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public int Deaths { get; private set; }
    [SerializeField] private GameObject _deadCanvas;

    public bool Dead { get; private set; }
    
    private void Start()
    {
        PlayerHealth.Instance.Dead += OnDead;
    }

    private void OnDead()
    {
        Deaths++;
        _deadCanvas.SetActive(true);
        Dead = true;
        GameCursor.Instance.Show();
        Time.timeScale = 0;
        AudioListener.volume = 0;
    }

    private void OnDisable()
    {
        Dead = false;
        Time.timeScale = 1;
        AudioListener.volume = 1;
        GameCursor.Instance.Hide();
        PlayerHealth.Instance.Dead -= OnDead;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}