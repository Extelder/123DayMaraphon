using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWaveSystem : MonoBehaviour
{
    [SerializeField] private EnemyWave[] _waves;

    [field: SerializeField] public int EnemyForOutline { get; private set; } = 5;

    public event Action WavingStarted;
    public event Action WavingEnded;


    public static EnemyWaveSystem CurrentWaveSystem { get; private set; }

    private EnemyWave _currentWave;


    public void StartWaves()
    {
        CurrentWaveSystem = this;
        StartCoroutine(Waving());
    }

    private IEnumerator Waving()
    {
        WavingStarted?.Invoke();
        for (int i = 0; i < _waves.Length; i++)
        {
            for (int j = 0; j < _waves[i].Enemies.Length; j++)
            {
                _waves[i].Enemies[j].SetActive(true);
            }

            _waves[i].WaveStarted?.Invoke();
            _currentWave = _waves[i];
            yield return new WaitUntil(() => AllGameObjectsNull(_waves[i].Enemies));
            _waves[i].WaveEnded?.Invoke();
        }

        WavingEnded?.Invoke();
    }

    private bool AllGameObjectsNull(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                return false;
            }
        }


        return true;
    }

    public int HowEnemyLost()
    {
        int lost = 0;


        for (int i = 0; i < _currentWave.Enemies.Length; i++)
        {
            if (_currentWave.Enemies[i] != null)
            {
                lost++;
            }
        }

        return lost;
    }
}

[Serializable]
public struct EnemyWave
{
    [field: SerializeField] public GameObject[] Enemies { get; private set; }
    public UnityEvent WaveEnded;
    public UnityEvent WaveStarted;
}