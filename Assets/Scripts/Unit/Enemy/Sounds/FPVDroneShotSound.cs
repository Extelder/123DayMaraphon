using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FPVDroneShotSound : MonoBehaviour
{
    [SerializeField] private FlyEnemyShooting _enemyPlayerAttackState;
    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _enemyPlayerAttackState.Attacked += OnAttacked;
    }

    private void OnAttacked()
    {
        _pools.FPVDroneShotSoundPool.GetFreeElement(transform.position, Quaternion.identity);
    }


    private void OnDisable()
    {
        _enemyPlayerAttackState.Attacked -= OnAttacked;
    }
}
