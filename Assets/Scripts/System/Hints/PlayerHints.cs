using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHints : MonoBehaviour
{
    [SerializeField] private Text _hintText;
    public void AcceptNewHint(string newHint)
    {
        _hintText.text = newHint;
    }
}
