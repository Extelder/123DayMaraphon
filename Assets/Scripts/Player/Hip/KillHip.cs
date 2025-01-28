using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHip : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _hitStateName = "Kill";

    private void OnEnable()
    {
        _animator.Play(_hitStateName);
    }
}