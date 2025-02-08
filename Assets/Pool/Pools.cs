using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    [field: SerializeField] public Pool DefaultImpactPool { get; private set; }
    [field: SerializeField] public Pool DefaultProjectilePool { get; private set; }
    [field: SerializeField] public Pool FPVProjectilePool { get; private set; }
    [field: SerializeField] public Pool TURRETProjectilePool { get; private set; }
    [field: SerializeField] public Pool GhostPool { get; private set; }
    [field: SerializeField] public Pool BloodExplodePool { get; private set; }
    [field: SerializeField] public Pool GhostBloodExplodePool { get; private set; }
    [field: SerializeField] public Pool DashDownPool { get; private set; }
    [field: SerializeField] public Pool DashPool { get; private set; }
    [field: SerializeField] public Pool TrailPool { get; private set; }
    [field: SerializeField] public Pool RailgunTrailPool { get; private set; }
    [field: SerializeField] public Pool BloodSplatPool { get; private set; }
    [field: SerializeField] public Pool HealPickupPool { get; private set; }
    [field: SerializeField] public Pool JumpSoundPool { get; private set; }
    [field: SerializeField] public Pool DashSoundPool { get; private set; }
    [field: SerializeField] public Pool ShotGunPool { get; private set; }
    [field: SerializeField] public Pool RailgunPool { get; private set; }
    [field: SerializeField] public Pool RiflePool { get; private set; }
    [field: SerializeField] public Pool RPGPool { get; private set; }
    [field: SerializeField] public Pool DashDownSoundPool { get; private set; }
    [field: SerializeField] public Pool BigBoySoundPool { get; private set; }
    [field: SerializeField] public Pool SexyGuyPunchSoundPool { get; private set; }
    [field: SerializeField] public Pool FPVDroneShotSoundPool { get; private set; }
    [field: SerializeField] public Pool TurretShotSoundPool { get; private set; }
    [field: SerializeField] public Pool RaperShotSoundPool { get; private set; }
    [field: SerializeField] public Pool GhostSoundPool { get; private set; }
    [field: SerializeField] public Pool ProjectileSoundPool { get; private set; }
}