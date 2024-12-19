using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HintTrigger : PlayerTrigger
{
    [SerializeField] private string _hintText;
    [Inject] private PlayerCharacter _character;
    public override void Triggered()
    { 
        _character.Hints.AcceptNewHint(_hintText);
    }
}
