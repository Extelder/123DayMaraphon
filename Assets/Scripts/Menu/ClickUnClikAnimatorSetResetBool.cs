using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnClikAnimatorSetResetBool : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool(_boolName, true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool(_boolName, false);
        }
    }
}