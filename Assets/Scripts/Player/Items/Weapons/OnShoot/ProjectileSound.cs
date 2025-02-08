using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileSound : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Pools _pools;

    private void OnEnable()
    {
        _projectile.Exploded += OnExploded;
    }

    private void OnExploded()
    {
        _pools.ProjectileSoundPool.GetFreeElement(transform.position, Quaternion.identity);
    }


    private void OnDisable()
    {
        _projectile.Exploded -= OnExploded;
    }
}
