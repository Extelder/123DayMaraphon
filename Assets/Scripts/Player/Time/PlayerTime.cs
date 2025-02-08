using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerTime : MonoBehaviour
{
    [SerializeField] private GameObject _stopTimeEffect;

    [Inject] private PlayerHealth _health;

    private Coroutine _currentCoroutine;

    public static PlayerTime Instance { get; private set; }

    private void OnEnable()
    {
        _health.Dead += OnDead;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }

    private void OnDead()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        _stopTimeEffect.SetActive(false);

        Debug.Break();
        Debug.LogError("There`s one more PlayerTime in scene");
        Debug.LogError(this);
    }

    public void PauseTimeStop()
    {
        StopAllCoroutines();
        Time.timeScale = 0;
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