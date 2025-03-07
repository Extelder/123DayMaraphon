using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingGameScrean : MonoBehaviour
{
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] private PlayerWarmingUp _playerWarmingUp;

    private void OnEnable()
    {
        _playerWarmingUp.WarmingUpEnded += OnWarmedUp;
    }

    private void OnWarmedUp()
    {
        _loadingObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        _playerWarmingUp.WarmingUpEnded -= OnWarmedUp;
    }
}
