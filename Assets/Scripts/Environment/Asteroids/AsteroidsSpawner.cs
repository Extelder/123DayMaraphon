using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _asteroidSpawnPoints;
    [SerializeField] private Pool _pool;
    [SerializeField] private float _minSpawnRate;
    [SerializeField] private float _maxSpawnRate;

    private void OnEnable()
    {
        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnRate, _maxSpawnRate));
            var randomPoint = _asteroidSpawnPoints[Random.Range(0, _asteroidSpawnPoints.Length)];
            _pool.GetFreeElement(randomPoint.position, randomPoint.rotation);
        }
    }
}
