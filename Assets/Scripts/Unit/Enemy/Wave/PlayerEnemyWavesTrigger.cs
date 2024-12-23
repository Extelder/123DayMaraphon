using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyWavesTrigger : PlayerTrigger
{
    [SerializeField] private EnemyWaveSystem _enemyWaveSystem;

    public override void Triggered()
    {
        _enemyWaveSystem.StartWaves();
    }
}