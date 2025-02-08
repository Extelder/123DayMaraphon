using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretShotSound : MonoBehaviour
{
    [SerializeField] private TurretShootState _enemyPlayerAttackState;
    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _enemyPlayerAttackState.Attacked += OnAttacked;
    }

    private void OnAttacked()
    {
        _pools.TurretShotSoundPool.GetFreeElement(transform.position, Quaternion.identity);
    }


    private void OnDisable()
    {
        _enemyPlayerAttackState.Attacked -= OnAttacked;
    }
}
