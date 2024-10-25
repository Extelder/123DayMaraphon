using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDashWarpEffect : MonoBehaviour
{
    [SerializeField] private PlayerDash _dash;
    [SerializeField] private Transform _spawnPoint;
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
        _pools.DashPool.GetFreeElement(_spawnPoint.position,
            _spawnPoint.rotation, _spawnPoint.transform);
    }
}