using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private string _name;
    private void Start()
    {
        _recordText.text = _name + PlayerPrefs.GetInt("MaxWave", 0);
    }
}
