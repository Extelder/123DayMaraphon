using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _skipText;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, TextAreaAttribute] private string _disclaimer;
    [SerializeField] private float _cooldown;

    private CompositeDisposable _disposable = new CompositeDisposable();
    private void Start()
    {
        StartCoroutine(SwitchDisclaimer());
        if (PlayerPrefs.GetInt("DisclaimerComplete", 0) == 1)
        {
            _skipText.SetActive(true);
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Input.anyKeyDown)
                {
                    _cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
                    StopAllCoroutines();
                    gameObject.SetActive(false);
                }
            }).AddTo(_disposable);
        }
    }

    private IEnumerator SwitchDisclaimer()
    {
        yield return new WaitForSeconds(_cooldown);
        _text.text = _disclaimer;
        yield return new WaitForSeconds(_cooldown);
        PlayerPrefs.SetInt("DisclaimerComplete", 1);
        _cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
