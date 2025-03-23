using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSHitBox : UnitHitBox
{
    [SerializeField] private Pool _explosionPool;

    public override void SpawningDecal(Vector3 spawnPoint)
    {
        var currentObject = _explosionPool.GetFreeElement(spawnPoint, Quaternion.identity);
    }
}