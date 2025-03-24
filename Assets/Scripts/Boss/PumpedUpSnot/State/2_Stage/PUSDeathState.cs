using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSDeathState : MonoBehaviour
{
    [SerializeField] private GameObject _flash;
    [SerializeField] private GameObject _deathAnimPus;
    [SerializeField] private GameObject _pus;

    [SerializeField] private Animator _animator;

    [SerializeField] private PUSHealth _health;

    private void OnEnable()
    {
        _health.Dead += OnDead;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }

    public void OnDead()
    {
        _animator.SetBool("IsDead", true);
    }

    public void DeathFlash()
    {
        _flash.SetActive(true);
        _deathAnimPus.SetActive(true);
        _pus.SetActive(false);
    }
}