using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField, TextAreaAttribute] private string _disclaimer;
    [SerializeField] private float _cooldown;

    private void Start()
    {
        StartCoroutine(SwitchDisclaimer());
    }

    private IEnumerator SwitchDisclaimer()
    {
        yield return new WaitForSeconds(_cooldown);
        _text.text = _disclaimer;
        yield return new WaitForSeconds(_cooldown);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
