using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGate : MonoBehaviour
{
    [SerializeField] private GameObject[] _encounterClearedObjects;

    private void Start()
    {
        StartCoroutine(WaitifngForClearAllEncounters());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitifngForClearAllEncounters()
    {
        yield return new WaitUntil(AllEncountersCleared);
        gameObject.SetActive(false);
    }

    private bool AllEncountersCleared()
    {
        for (int i = 0; i < _encounterClearedObjects.Length; i++)
        {
            if (_encounterClearedObjects[i].activeSelf == true)
                return false;
        }

        return true;
    }
}