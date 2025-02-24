using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyShootingMuzzleFlash : MonoBehaviour
{
    [SerializeField] private FlyEnemyShooting _flyEnemyShooting;
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void OnEnable()
    {
        _flyEnemyShooting.Attacked += PlayMuzzleflash;
    }

    private void OnDisable()
    {
        _flyEnemyShooting.Attacked -= PlayMuzzleflash;
    }

    private void PlayMuzzleflash()
    {
        _muzzleFlash.Play();
    }
}