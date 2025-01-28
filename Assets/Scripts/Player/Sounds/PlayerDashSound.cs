using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDashSound : MonoBehaviour
{
    [SerializeField] private PlayerDash _dash;

    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _dash.Dashed += OnDashed;
    }

    private void OnDisable()
    {
        _dash.Dashed -= OnDashed;
    }

    private void OnDashed()
    {
        _pools.DashSoundPool.GetFreeElement(transform.position);
    }
}