using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parent;

    public override void InstallBindings()
    {
        var playerInstance = Container.InstantiatePrefabForComponent<PlayerCharacter>(_playerCharacter,
            _spawnPoint.position, Quaternion.identity, _parent);

        Container.Bind<PlayerCharacter>().FromInstance(playerInstance).AsSingle().NonLazy();
        Container.QueueForInject(playerInstance);
    }
}