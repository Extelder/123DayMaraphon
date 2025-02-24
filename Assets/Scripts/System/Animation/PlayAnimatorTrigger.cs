using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimatorTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorName;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        _animator.SetTrigger(_animatorName);
    }
}
