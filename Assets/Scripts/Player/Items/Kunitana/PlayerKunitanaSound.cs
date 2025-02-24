using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerKunitanaSound : MonoBehaviour
{
    [SerializeField] private KunitanShoot _kunitanaShoot;

    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _kunitanaShoot.Shooted += OnAttacked;
    }

    private void OnDisable()
    {
        _kunitanaShoot.Shooted -= OnAttacked;
    }

    private void OnAttacked()
    {
        _pools.KatanaSoundPool.GetFreeElement(transform.position);
    }
}
