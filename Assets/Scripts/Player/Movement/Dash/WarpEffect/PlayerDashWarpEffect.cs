using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDashWarpEffect : MonoBehaviour
{
    [SerializeField] private PlayerDash _dash;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Pool _warpEffectPool;

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
        _warpEffectPool.GetFreeElement(_spawnPoint.position,
            _spawnPoint.rotation, _spawnPoint.transform);
    }
}