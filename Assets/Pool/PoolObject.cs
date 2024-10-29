using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [field: SerializeField] public float ReturnToPoolDelay { get; private set; } = 2f;
    [SerializeField] private bool _autoreturnToPool = true;

    private void OnEnable()
    {
        if (_autoreturnToPool)
            Invoke("ReturnToPool", ReturnToPoolDelay);
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnCreate()
    {
    }
}