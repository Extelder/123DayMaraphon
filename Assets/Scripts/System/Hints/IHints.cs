using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IHints : MonoBehaviour
{
    [SerializeField] private Text _hint;
    [SerializeField] private PlayerTextTrigger _textTrigger;

    private void OnEnable()
    {
        _textTrigger.PlayerTriggered += ShowHint;
        _textTrigger.PlayerUntriggered += HideHint;
    }

    private void ShowHint()
    {
        _hint.gameObject.SetActive(true);
    }
    private void HideHint()
    {
        _hint.gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        _textTrigger.PlayerTriggered -= ShowHint;
        _textTrigger.PlayerTriggered -= HideHint;
    }
}
