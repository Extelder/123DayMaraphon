using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public Pool GauskaPool;
    public Pool RocketsPool;
    public Pool BulletsPool;
    public Pool JumpadsPool;
    public Pool EnemyDamageEffectPool;

    public static Pools Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Destroy(GauskaPool);
        Destroy(RocketsPool);
        Destroy(BulletsPool);
        Destroy(JumpadsPool);
        Destroy(EnemyDamageEffectPool);
        Destroy(this);
    }
}