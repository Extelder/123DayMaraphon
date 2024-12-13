using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSystem : MonoBehaviour
{
    [SerializeField] private EnemyWave[] _waves;

    public event Action WavingStarted;
    public event Action WavingEnded;

    public void StartWaves()
    {
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

            yield return new WaitUntil(() => AllGameObjectsNull(_waves[i].Enemies));
        }

        WavingEnded?.Invoke();
    }

    private bool AllGameObjectsNull(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                return false;
        }

        return true;
    }
}

[Serializable]
public struct EnemyWave
{
    [field: SerializeField] public GameObject[] Enemies { get; private set; }
}