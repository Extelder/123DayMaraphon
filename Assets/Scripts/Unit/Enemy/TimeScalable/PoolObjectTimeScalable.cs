using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectTimeScalable : PoolObject
{
    public override void ReturnToPool()
    {
        if (KunitanaUltimate.Instance.Ultimating)
        {
            Invoke(nameof(ReturnToPool), 2f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
