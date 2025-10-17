using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public struct Spawnable
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int MinToSpawn { get; private set; }
    [field: SerializeField] public int MaxToSpawn { get; private set; }
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave _wave;

    [SerializeField] private Spawnable[] _spawnable;
    [SerializeField] private float _defaultRate;
    [SerializeField] private float _rateDecreasePerWave;
    [SerializeField] private int _waveToStart;

    private float _startRate;
    private List<GameObject> _spawned = new List<GameObject>();


    private void Start()
    {
        _startRate = _defaultRate;
        _defaultRate = _startRate;
    }

    private void OnEnable()
    {
        _wave.Started += OnStarted;
        _wave.Ended += OnEnded;
    }


    private void OnStarted(int value)
    {
        if (value >= _waveToStart)
        {
            _defaultRate -= _rateDecreasePerWave;
            StartCoroutine(Spawning());
        }
    }

    private IEnumerator Spawning()
    {
        _spawned.Clear();

        while (true)
        {
            yield return new WaitForSeconds(_defaultRate);
            for (int i = 0; i < _spawnable.Length; i++)
            {
                for (int j = 0; j < Random.Range(_spawnable[i].MinToSpawn, _spawnable[i].MaxToSpawn); j++)
                {
                    Vector3 spawnPosition = transform.position + 
                        new Vector3(Random.Range(-_wave.DefaultSpawnRadius.x, _wave.DefaultSpawnRadius.x),
                            _wave.DefaultSpawnRadius.y,
                            Random.Range(-_wave.DefaultSpawnRadius.z, _wave.DefaultSpawnRadius.z));

                    RaycastHit hit;
                    if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 100000f, LayerMask.GetMask("Default")))
                    {
                        _spawned.Add(Instantiate(_spawnable[i].Prefab, hit.point, Quaternion.identity));
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        _defaultRate = _startRate;
        _wave.Started -= OnStarted;
        _wave.Ended -= OnEnded;
    }

    private void OnEnded(int value)
    {
        _defaultRate = _startRate;
        if (value >= _waveToStart)
        {
            StopAllCoroutines();

            for (int i = 0; i < _spawned.Count; i++)
            {
                if (_spawned[i] == null)
                    continue;
                Destroy(_spawned[i]);
            }
        }
    }
}