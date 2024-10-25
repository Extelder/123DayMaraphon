using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolsInstaller : MonoInstaller
{
    [SerializeField] private Pools _pools;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parent;

    public override void InstallBindings()
    {
        var poolsInstance = Container.InstantiatePrefabForComponent<Pools>(_pools,
            _spawnPoint.position, Quaternion.identity, _parent);

        Container.Bind<Pools>().FromInstance(poolsInstance).AsSingle().NonLazy();
        Container.QueueForInject(poolsInstance);
    }
}