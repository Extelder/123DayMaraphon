using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoInstaller
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Transform _point;

    private void Start()
    {
        var enemy = Instantiate(_enemy, _point.position, Quaternion.identity);
    }
}