using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HubButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _socialNetworks;
    [SerializeField] private string _textToShow;
    [SerializeField] private string _textToHide;

    public void TryHideSocialNetworks()
    {
        if (_text.text == _textToHide)
        {
            _socialNetworks.SetActive(false);
            _text.text = _textToShow;
            return;
        }

        TryShowSocialNetworks();
    }

    private void TryShowSocialNetworks()
    {
        if (_text.text == _textToShow)
        {
            _socialNetworks.SetActive(true);
            _text.text = _textToHide;
            return;
        }
    }
}
