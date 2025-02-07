using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject _deadCanvas;

    private void Start()
    {
        PlayerHealth.Instance.Dead += OnDead;
    }

    private void OnDead()
    {
        _deadCanvas.SetActive(true);
        GameCursor.Instance.Show();
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        GameCursor.Instance.Hide();
        PlayerHealth.Instance.Dead -= OnDead;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}