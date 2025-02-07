using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : MonoBehaviour
{
    [SerializeField] private GameObject _stopTimeEffect;

    private Coroutine _currentCoroutine;

    public static PlayerTime Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.Break();
        Debug.LogError("There`s one more PlayerTime in scene");
        Debug.LogError(this);
    }

    public void TimeStop(float time)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            Time.timeScale = 1;
        }

        _currentCoroutine = StartCoroutine(Stopping(time));
    }

    private IEnumerator Stopping(float time)
    {
        _stopTimeEffect.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        if (!PlayerHealth.Instance.IsDead)
            Time.timeScale = 1;
        _stopTimeEffect.SetActive(false);
    }
}