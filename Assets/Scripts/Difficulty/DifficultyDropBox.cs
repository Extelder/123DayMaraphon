using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyDropBox : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropDown;

    private DifficultyHandler _difficulty;

    private void Start()
    {
        _difficulty = DifficultyHandler.Instance;

        _dropDown.value = PlayerPrefs.GetInt("Difficulty", 1) - 1;
    }

    private void OnEnable()
    {
        _dropDown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                _difficulty.Easy();
                break;
            case 1:
                _difficulty.Medium();
                break;
            case 2:
                _difficulty.Hard();
                break;
        }
    }

    private void OnDisable()
    {
        _dropDown.onValueChanged.RemoveListener(OnValueChanged);
    }
}