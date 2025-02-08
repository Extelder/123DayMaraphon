using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GhostSound : MonoBehaviour
{
    [SerializeField] private Pools _pools;

    private void OnEnable()
    {
        _pools.GhostSoundPool.GetFreeElement(transform.position, Quaternion.identity, transform);
    }
}