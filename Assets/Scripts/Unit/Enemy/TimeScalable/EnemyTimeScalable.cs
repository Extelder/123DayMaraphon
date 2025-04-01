using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyTimeScalable : MonoBehaviour
{
    [SerializeField] private float _animatorSpeed;
    private Animator _animator;
    private float _defaultAnimatorSpeed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _defaultAnimatorSpeed = _animator.speed;
    }

    private void OnEnable()
    {
        if (KunitanaUltimate.Instance.Ultimating)
        {
            OnUltimated();
        }
        KunitanaUltimate.Ultimated += OnUltimated;
        KunitanaUltimate.UltimateStoped += OnUltimateStoped;
    }

    private void OnUltimated()
    {
        _animator.speed = _animatorSpeed;
    }

    private void OnUltimateStoped()
    {
        _animator.speed = _defaultAnimatorSpeed;
    }

    private void OnDisable()
    {
        KunitanaUltimate.Ultimated -= OnUltimated;
        KunitanaUltimate.UltimateStoped -= OnUltimateStoped;
    }
}
