using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseEnterExitChangeAnimatorBool : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName = "MouseOn";

    public UnityEvent OnMouseDowned;
    public UnityEvent OnMouseEntered;
    public UnityEvent OnMouseExited;

    private void OnMouseEnter()
    {
        OnMouseEntered?.Invoke();
        _animator.SetBool(_boolName, true);
    }

    private void OnMouseExit()
    {
        OnMouseExited?.Invoke();
        _animator.SetBool(_boolName, false);
    }

    private void OnMouseDown()
    {
        OnMouseDowned?.Invoke();
    }
}