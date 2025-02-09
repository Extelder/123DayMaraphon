using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionAnimator : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggername;

    private void OnEnable()
    {
        _projectile.Exploded += OnExploded;
    }

    private void OnExploded()
    {
        _animator.SetBool(_triggername, true);
    }


    private void OnDisable()
    {
        _animator.SetBool(_triggername, false);
        _projectile.Exploded -= OnExploded;
    }
}