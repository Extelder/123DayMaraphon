using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private GameObject _pressKeyHint;

    private int _currentSceneIndex;
    private AsyncOperation _asyncOperation;


    private void Start()
    {
        StopAllCoroutines();
        _currentSceneIndex = PlayerPrefs.GetInt("CurrentScene", 0);
        StartCoroutine(LoadingScene());
    }


    private IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(1f);
        float loadingProgress;
        _asyncOperation = SceneManager.LoadSceneAsync(_currentSceneIndex);

        _asyncOperation.allowSceneActivation = false;
        while (_asyncOperation.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
            loadingProgress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            _loadingText.text = $"Loading ... {(loadingProgress * 100).ToString("0")}%";
            yield return true;
        }

        _pressKeyHint.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown && _pressKeyHint.activeSelf)
        {
            _asyncOperation.allowSceneActivation = true;
        }
    }
}