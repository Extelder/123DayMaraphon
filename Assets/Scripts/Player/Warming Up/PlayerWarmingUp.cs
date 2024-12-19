using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarmingUp : MonoBehaviour
{
    [SerializeField] private GameObject[] _warmingUpWeapons;
    [SerializeField] private float _warmingUpTime;

    public event Action WarmingUpStarted;
    public event Action WarmingUpEnded;

    private void Start()
    {
        StartCoroutine(WarmingUp());
    }

    private IEnumerator WarmingUp()
    {
        WarmingUpStarted?.Invoke();
        for (int i = 0; i < _warmingUpWeapons.Length; i++)
        {
            _warmingUpWeapons[i].SetActive(true);
        }

        yield return new WaitForSeconds(_warmingUpTime);
        for (int i = 0; i < _warmingUpWeapons.Length; i++)
        {
            _warmingUpWeapons[i].SetActive(false);
        }

        WarmingUpEnded?.Invoke();
    }
}