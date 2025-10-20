using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HintTrigger : PlayerTrigger
{
    [SerializeField] private TextMeshProUGUI _text;
    
    [SerializeField] private string _hintText;
    [Inject] private PlayerCharacter _character;
    public override void OnTriggered()
    { 
        _text.text = _hintText;
        _character.Hints.AcceptNewHint(_hintText);
    }
}
