using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private float _returnToPoolDelay = 2f;

    private void OnEnable() => Invoke("ReturnToPool", _returnToPoolDelay);

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnCreate()
    {
    }
}