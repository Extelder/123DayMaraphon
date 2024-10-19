using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputInstaller : MonoInstaller
{
    [SerializeField] private PlayerInputs _playerInputs;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parent;

    public override void InstallBindings()
    {
        var playerInstance = Container.InstantiatePrefabForComponent<PlayerInputs>(_playerInputs,
            _spawnPoint.position, Quaternion.identity, _parent);

        Container.Bind<PlayerInputs>().FromInstance(playerInstance).AsSingle().NonLazy();
        Container.QueueForInject(playerInstance);
    }
}