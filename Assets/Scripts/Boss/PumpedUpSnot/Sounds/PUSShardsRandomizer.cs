using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PUSShardsRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] _shardsVariants;

    private GameObject _currentShard;

    private void OnEnable()
    {
        _currentShard?.SetActive(false);
        RandomizeShards();
    }

    private void RandomizeShards()
    {
        _currentShard = _shardsVariants[Random.Range(0, _shardsVariants.Length)];
        _currentShard.SetActive(true);
    }
}
