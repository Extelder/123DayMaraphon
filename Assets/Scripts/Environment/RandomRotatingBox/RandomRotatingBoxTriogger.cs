using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotatingBoxTriogger : PlayerTrigger
{
    [SerializeField] private RandomRotatingBox _randomRotatingBox;

    [SerializeField] private GameObject _enter;

    public override void Triggered()
    {
        _randomRotatingBox.enabled = true;
        _enter.SetActive(true);
    }
}