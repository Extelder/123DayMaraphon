using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealthInstaller : MonoInstaller
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parent;

    public override void InstallBindings()
    {
        var playerHealthInstance = Container.InstantiatePrefabForComponent<PlayerHealth>(_playerHealth,
            _spawnPoint.position, Quaternion.identity, _parent);

        Container.Bind<PlayerHealth>().FromInstance(playerHealthInstance).AsSingle().NonLazy();
        Container.QueueForInject(playerHealthInstance);
    }
}