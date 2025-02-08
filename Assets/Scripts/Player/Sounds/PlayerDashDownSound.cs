using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDashDownSound : MonoBehaviour
{
    
    [SerializeField] private PlayerDashDown _dashDown;

    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _dashDown.Stashed += OnDashed;
    }

    private void OnDisable()
    {
        _dashDown.Stashed -= OnDashed;
    }

    private void OnDashed()
    {
        _pools.DashDownSoundPool.GetFreeElement(transform.position);
    }
}
